using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mixter.Infrastructure;
using Mixter.Infrastructure.Repositories;
using NFluent;

namespace Mixter.Tests.Infrastructure
{
    [TestClass]
    public class EventHandlersGeneratorTest
    {
        [TestMethod]
        public void WhenGenerateThenCreateAllEventHandlers()
        {
            var generator = new EventHandlersGenerator(new EventsDatabase(), new TimelineMessagesRepository());

            var handlers = generator.Generate(new EventPublisher()).ToArray();

            var handlersOfAssembly = typeof (EventHandlersGenerator).Assembly.GetTypes()
                                                                    .Where(o => o.IsClass)
                                                                    .Where(o => typeof(IEventHandler).IsAssignableFrom(o))
                                                                    .ToArray();
            Check.That(handlersOfAssembly).Not.IsEmpty();
            Check.That(handlers).HasSize(handlersOfAssembly.Length);
            Check.That(handlers.Select(o => o.GetType())).Contains(handlersOfAssembly);
        }
    }
}