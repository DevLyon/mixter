using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Mixter.Domain;
using Mixter.Infrastructure;
using Mixter.Web.BoundedContexts;

var builder = WebApplication.CreateBuilder(args);

var eventsStore = new EventsStore();

var eventPublisher = new EventPublisher();
var eventPublisherWithStorage = new EventPublisherWithStorage(eventsStore, eventPublisher);

builder.Services.AddSingleton<IEventPublisher>(eventPublisherWithStorage);

Identity.InitializeDependencies(builder.Services, eventsStore, eventPublisher);

var app = builder.Build();

app.Use((context, next) =>
{
    context.Request.Headers.TryGetValue("Authorization", out var value);
    var sessionId = value.FirstOrDefault();
    if (sessionId != null)
    {
        context.User = new ClaimsPrincipal(new ClaimsIdentity(
            new List<Claim> { new Claim(ClaimTypes.NameIdentifier, sessionId) },
            nameType: ClaimTypes.NameIdentifier, 
            roleType: ClaimTypes.Role, 
            authenticationType: "session"
        ));
    }
    
    return next(context);
});

app.MapGet("/", () => Task.FromResult("Hello World, it's Mixter on .NET Core"));

app.MapGroup("/api/identity").MapIdentityRoutes();

app.Run();