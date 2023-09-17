using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Mixter.Domain;
using Mixter.Domain.Core.Messages;
using Mixter.Domain.Core.Messages.Handlers;
using Mixter.Domain.Core.Subscriptions.Handlers;
using Mixter.Domain.Identity;
using Mixter.Infrastructure;

namespace Mixter.Web.BoundedContexts;

public static class Core
{
    public static void InitializeDependencies(IServiceCollection services, EventsStore eventsStore, IEventPublisher eventPublisher, IEventSubscriber eventSubscriber)
    {
        var timelineMessageRepository = new TimelineMessageRepository();
        var followersRepository = new FollowersRepository();
        var subscriptionsRepository = new SubscriptionsRepository(eventsStore, followersRepository);

        eventSubscriber.Subscribe(new UpdateTimeline(timelineMessageRepository));
        eventSubscriber.Subscribe(new NotifyFollowerOfFolloweeMessage(followersRepository, subscriptionsRepository, eventPublisher));
        eventSubscriber.Subscribe(new UpdateFollowers(followersRepository));

        services.AddSingleton<ITimelineMessageRepository>(timelineMessageRepository);
        services.AddSingleton<IMessagesRepository>(new MessagesRepository(eventsStore));
    }
    
    public static void MapCoreRoutes(this RouteGroupBuilder routes)
    {
        routes.MapPost("/messages/quack", (ClaimsPrincipal user, [FromBody] QuackMessage command, IEventPublisher eventPublisher, ISessionsRepository sessionsRepository) =>
        {
            if (user == null || user.Identity == null) return Results.Forbid();
        
            var author = sessionsRepository.GetUserIdOfSession(new SessionId(user.Identity.Name));
            if (author == null) return Results.Unauthorized();
        
            var messageId = Message.Quack(eventPublisher, author.Value, command.Content);

            return Results.Created("/api/core/messages/" + Uri.EscapeDataString(messageId.ToString()), messageId);
        });

        routes.MapDelete("/messages/{messageId}", (ClaimsPrincipal user, string messageId, IMessagesRepository messagesRepository, ISessionsRepository sessionsRepository, IEventPublisher eventPublisher) =>
        {
            if (user == null || user.Identity == null) return Results.Forbid();
        
            var sessionId = new SessionId(user.Identity.Name);

            var deleter = sessionsRepository.GetUserIdOfSession(sessionId);
            if (deleter == null) return Results.Unauthorized();

            var messageToDeleted = messagesRepository.Get(new MessageId(messageId));

            messageToDeleted.Delete(eventPublisher, deleter.Value);

            return Results.Ok("Message deleted");
        });

        routes.MapGet("/timelineMessages/{author}", (string author, ITimelineMessageRepository timelineMessageRepository) =>
        {
            var messages = timelineMessageRepository.GetMessagesOfUser(new UserId(author));

            return Results.Ok(messages);
        });
    }

    class QuackMessage
    {
        public string Content { get; set; }
    }
}