using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Mixter.Domain;
using Mixter.Domain.Identity;
using Mixter.Infrastructure;

namespace Mixter.Web.BoundedContexts;

public static class Identity
{
    public static void InitializeDependencies(IServiceCollection services, EventsStore eventsStore, IEventSubscriber eventSubscriber)
    {
        var sessionsRepository = new SessionsRepository(eventsStore);

        eventSubscriber.Subscribe(new SessionHandler(sessionsRepository));

        services.AddSingleton<IUserIdentitiesRepository>(new UserIdentitiesRepository(eventsStore));
        services.AddSingleton<ISessionsRepository>(sessionsRepository);
    }
    
    public static void MapIdentityRoutes(this RouteGroupBuilder routes)
    {
        routes.MapPost("/userIdentities/register", ([FromBody] RegisterUser command, IEventPublisher eventPublisher) =>
        {
            var userId = new UserId(command.Email);

            UserIdentity.Register(eventPublisher, userId);

            return Results.Created("/api/identity/userIdentities/" + Uri.EscapeDataString(userId.ToString()), new
            {
                Id = userId,
                Url = "/api/identity/userIdentities/" + Uri.EscapeDataString(userId.ToString()),
                LogIn = "/api/identity/userIdentities/" + Uri.EscapeDataString(userId.ToString()) + "/logIn"
            });
        });
    
        routes.MapPost("/userIdentities/{userId}/logIn", (string userId, IUserIdentitiesRepository userIdentitiesRepository, IEventPublisher eventPublisher) =>
        {
            var userIdentity = userIdentitiesRepository.GetUserIdentity(new UserId(userId));

            var sessionId = userIdentity.LogIn(eventPublisher);

            return Results.Created("/api/identity/sessions/" + Uri.EscapeDataString(sessionId.ToString()), sessionId);
        });

        routes.MapPost("/sessions/disconnect", (ClaimsPrincipal user, ISessionsRepository sessionsRepository, IEventPublisher eventPublisher) =>
        {
            if (user == null || user.Identity == null) return Results.Forbid();
        
            var session = sessionsRepository.GetSession(new SessionId(user.Identity.Name));

            session.Logout(eventPublisher);

            return Results.NoContent();
        });
    }

    class RegisterUser
    {
        public string Email { get; set; }
    }
}