using System;
using Mixter.Domain;
using Mixter.Domain.Identity;
using Nancy;
using Nancy.ModelBinding;

namespace Mixter.Web
{
    public class IdentityApi : NancyModule
    {
        public IdentityApi(IEventPublisher eventPublisher)
        {
            Post("/api/identity/userIdentities/register", _ => Execute(eventPublisher, this.Bind<RegisterUser>()));
        }

        private dynamic Execute(IEventPublisher eventPublisher, RegisterUser command)
        {
            var userId = new UserId(command.Email);

            UserIdentity.Register(eventPublisher, userId);

            return Negotiate.WithStatusCode(HttpStatusCode.Created).WithModel(new
            {
                Id = userId,
                Url = "/api/identity/userIdentities/" + Uri.EscapeUriString(userId.Email),
                LogIn = "/api/identity/userIdentities/" + Uri.EscapeUriString(userId.Email) + "/logIn"
            });
        }

        private class RegisterUser
        {
            public string Email { get; set; }
        }
    }
}