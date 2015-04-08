using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mixter.Domain.Core.Messages;
using Mixter.Domain.Core.Messages.Events;
using Mixter.Domain.Identity;
using Mixter.Infrastructure.Repositories;
using NFluent;

namespace Mixter.Infrastructure.Tests.Infrastructure.Repositories
{
    [TestClass]
    public class MessagesRepositoryTest
    {
        private EventsStore _eventsStore;
        private MessagesRepository _repository;

        [TestInitialize]
        public void Initialize()
        {
            _eventsStore = new EventsStore();
            _repository = new MessagesRepository(_eventsStore);
        }

        [TestMethod]
        public void GivenMessagePublishedThenGetMessageThenReturnTheMessage()
        {
            var messagePublished = new MessagePublished(MessageId.Generate(), new UserId("bob@mixit.fr"), "Hello");
            _eventsStore.Store(messagePublished);

            var message = _repository.Get(messagePublished.Id);

            Check.That(message.GetId()).IsEqualTo(messagePublished.Id);
        }

        [TestMethod]
        public void GivenMessagePublishedThenGetDescriptionThenReturnMessageDescription()
        {
            var messagePublished = new MessagePublished(MessageId.Generate(), new UserId("bob@mixit.fr"), "Hello");
            _eventsStore.Store(messagePublished);

            var description = _repository.GetDescription(messagePublished.Id);

            Check.That(description.Author).IsEqualTo(messagePublished.Author);
            Check.That(description.Content).IsEqualTo(messagePublished.Content);
        }

        [TestMethod]
        public void GivenReplyMessagePublishedThenGetDescriptionThenReturnMessageDescription()
        {
            var replyMessagePublished = new ReplyMessagePublished(MessageId.Generate(), new UserId("bob@mixit.fr"), "Hello", MessageId.Generate());
            _eventsStore.Store(replyMessagePublished);

            var description = _repository.GetDescription(replyMessagePublished.ReplyId);

            Check.That(description.Author).IsEqualTo(replyMessagePublished.Replier);
            Check.That(description.Content).IsEqualTo(replyMessagePublished.ReplyContent);
        }
    }
}
