using Microsoft.AspNetCore.Builder;
using Nancy.Owin;

namespace Mixter.Web
{
    public class Startup
    {
        public void Configure(IApplicationBuilder app)
        {
            app.UseOwin(owin => owin.UseNancy(options => options.Bootstrapper = new Bootstrapper()));
        }
    }
}