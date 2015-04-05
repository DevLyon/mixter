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
        private EventsDatabase _eventsDatabase;
        private MessagesRepository _repository;

        [TestInitialize]
        public void Initialize()
        {
            _eventsDatabase = new EventsDatabase();
            _repository = new MessagesRepository(_eventsDatabase);
        }

        [TestMethod]
        public void GivenMessagePublishedThenGetMessageThenReturnTheMessage()
        {
            var messagePublished = new MessagePublished(MessageId.Generate(), new UserId("bob@mixit.fr"), "Hello");
            _eventsDatabase.Store(messagePublished);

            var message = _repository.Get(messagePublished.Id);

            Check.That(message.GetId()).IsEqualTo(messagePublished.Id);
        }

        [TestMethod]
        public void GivenMessagePublishedThenGetDescriptionThenReturnMessageDescription()
        {
            var messagePublished = new MessagePublished(MessageId.Generate(), new UserId("bob@mixit.fr"), "Hello");
            _eventsDatabase.Store(messagePublished);

            var description = _repository.GetDescription(messagePublished.Id);

            Check.That(description.Author).IsEqualTo(messagePublished.Author);
            Check.That(description.Content).IsEqualTo(messagePublished.Content);
        }
    }
}
