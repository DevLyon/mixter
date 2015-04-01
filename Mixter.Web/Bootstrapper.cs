using System;
using System.IO;
using Nancy;
using Nancy.Conventions;
using Nancy.Helpers;
using Nancy.Responses;

namespace Mixter.Web
{
    public class Bootstrapper : DefaultNancyBootstrapper
    {
        private const string PublicDirectory = @"../../../public/";

        protected override void ConfigureConventions(NancyConventions conventions)
        {
            base.ConfigureConventions(conventions);

            conventions.StaticContentsConventions.Clear();
            conventions.StaticContentsConventions.Add(AddDirectory(PublicDirectory));

            conventions.ViewLocationConventions.Add((viewName, model, context) => string.Concat(PublicDirectory, viewName));
        }

        private static Func<NancyContext, string, Response> AddDirectory(string contentPath)
        {
            GenericFileResponse.SafePaths.Add(Path.GetFullPath(contentPath));

            return (ctx, root) => BuildContentDelegate(ctx, contentPath);
        }

        private static Response BuildContentDelegate(NancyContext nancyContext, string contentPath)
        {
            var path = HttpUtility.UrlDecode(nancyContext.Request.Path);
            if (string.IsNullOrEmpty(Path.GetFileName(path)))
            {
                return null;
            }

            var fileName = Path.GetFullPath(contentPath + path);

            return File.Exists(fileName) 
                       ? new GenericFileResponse(fileName, nancyContext) 
                       : null;
        }
    }
}