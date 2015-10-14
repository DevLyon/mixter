using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Mixter.Domain;
using Mixter.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

var eventsStore = new EventsStore();

var eventPublisher = new EventPublisher();
var eventPublisherWithStorage = new EventPublisherWithStorage(eventsStore, eventPublisher);

builder.Services.AddSingleton<IEventPublisher>(eventPublisherWithStorage);

var app = builder.Build();

app.MapGet("/", () => Task.FromResult("Hello World, it's Mixter on .NET Core"));

app.Run();