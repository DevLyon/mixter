using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mixter.Domain.Core;
using Mixter.Domain.Core.Messages;
using Mixter.Domain.Core.Messages.Events;
using Mixter.Domain.Core.Subscriptions;
using Mixter.Domain.Core.Subscriptions.Events;
using Mixter.Domain.Core.Subscriptions.Handlers;
using Mixter.Infrastructure;
using Mixter.Infrastructure.Repositories;
using NFluent;

namespace Mixter.Tests.Domain.Core.Subscriptions
{
    [TestClass]
    public class AddMessageOnFollowerTimelineTest
    {
        private TimelineMessagesRepository _timelineMessageRepository;
        private EventsDatabase _database;
        private AddMessageOnFollowerTimeline _handler;

        [TestInitialize]
        public void Initialize()
        {
            _timelineMessageRepository = new TimelineMessagesRepository();
            _database = new EventsDatabase();
            _handler = new AddMessageOnFollowerTimeline(_database, _timelineMessageRepository);
        }

        [TestMethod]
        public void WhenHandleFollowerMessagePublishedThenAddMessageOnFollowerTimeline()
        {
            var followee = new UserId("followee@mixit.fr");
            var follower = new UserId("follower@mixit.fr");
            var messagePublished = PublishMessage(followee, "Hello");

            _handler.Handle(new FollowerMessagePublished(new SubscriptionId(follower, followee), messagePublished.Id));

            var messages = _timelineMessageRepository.GetMessagesOfUser(follower);
            Check.That(messages).HasSize(1)
                 .And.ContainsExactly(new TimelineMessage(follower, messagePublished));
        }

        private MessagePublished PublishMessage(UserId author, string content)
        {
            var messagePublished = new MessagePublished(MessageId.Generate(), author, content);
            _database.Store(messagePublished);

            return messagePublished;
        }
    }
}