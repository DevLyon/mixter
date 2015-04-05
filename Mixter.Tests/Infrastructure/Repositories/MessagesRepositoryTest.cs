using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mixter.Domain.Core.Messages;
using Mixter.Domain.Core.Messages.Events;
using Mixter.Domain.Identity;
using Mixter.Infrastructure;
using Mixter.Infrastructure.Repositories;
using NFluent;

namespace Mixter.Tests.Infrastructure.Repositories
{
    [TestClass]
    public class MessagesRepositoryTest
    {
        [TestMethod]
        public void GivenMessagePublishedThenGetMessageThenReturnTheMessage()
        {
            var eventsDatabase = new EventsDatabase();
            var repository = new MessagesRepository(eventsDatabase);
            var messagePublished = new MessagePublished(MessageId.Generate(), new UserId("bob@mixit.fr"), "Hello");
            eventsDatabase.Store(messagePublished);

            var message = repository.Get(messagePublished.Id);

            Check.That(message.GetId()).IsEqualTo(messagePublished.Id);
        }

        [TestMethod]
        public void GivenMessagePublishedThenGetDescriptionThenReturnMessageDescription()
        {
            var eventsDatabase = new EventsDatabase();
            var repository = new MessagesRepository(eventsDatabase);
            var messagePublished = new MessagePublished(MessageId.Generate(), new UserId("bob@mixit.fr"), "Hello");
            eventsDatabase.Store(messagePublished);

            var description = repository.GetDescription(messagePublished.Id);

            Check.That(description.Author).IsEqualTo(messagePublished.Author);
            Check.That(description.Content).IsEqualTo(messagePublished.Content);
        }
    }
}
