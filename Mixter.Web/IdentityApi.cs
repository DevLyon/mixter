using System;
using Mixter.Domain;
using Mixter.Domain.Identity;
using Nancy;
using Nancy.ModelBinding;

namespace Mixter.Web
{
    public class IdentityApi : NancyModule
    {
        public IdentityApi(IEventPublisher eventPublisher, IUserIdentitiesRepository userIdentitiesRepository)
        {
            Post("/api/identity/userIdentities/register", _ => Execute(eventPublisher, this.Bind<RegisterUser>()));
            Post("/api/identity/userIdentities/{id}/logIn", _ => Execute(eventPublisher, userIdentitiesRepository, new LogInUser { UserId = new UserId(_.Id)}));
        }

        private dynamic Execute(IEventPublisher eventPublisher, RegisterUser command)
        {
            var userId = new UserId(command.Email);

            UserIdentity.Register(eventPublisher, userId);

            return Negotiate.WithStatusCode(HttpStatusCode.Created).WithModel(new
            {
                Id = userId,
                Url = "/api/identity/userIdentities/" + Uri.EscapeUriString(userId.ToString()),
                LogIn = "/api/identity/userIdentities/" + Uri.EscapeUriString(userId.ToString()) + "/logIn"
            });
        }

        private object Execute(IEventPublisher eventPublisher, IUserIdentitiesRepository userIdentitiesRepository, LogInUser command)
        {
            var userIdentity = userIdentitiesRepository.GetUserIdentity(command.UserId);

            var sessionId = userIdentity.LogIn(eventPublisher);

            return Negotiate.WithStatusCode(HttpStatusCode.Created).WithModel(new
            {
                Id = sessionId,
                LogOut = "/api/identity/sessions/" + Uri.EscapeUriString(sessionId.ToString()) + "/logOut"
            });
        }

        private class RegisterUser
        {
            public string Email { get; set; }
        }

        private class LogInUser
        {
            public UserId UserId { get; set; }
        }
    }
}