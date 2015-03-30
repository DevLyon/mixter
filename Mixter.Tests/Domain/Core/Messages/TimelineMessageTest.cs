using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mixter.Domain.Core.Messages;
using Mixter.Domain.Core.Messages.Events;
using Mixter.Domain.Identity;
using Mixter.Tests.Infrastructure;
using NFluent;

namespace Mixter.Tests.Domain.Core.Messages
{
    [TestClass]
    public class TimelineMessageTest
    {
        [TestMethod]
        public void WhenPublishTimelineMessageThenRaiseTimelineMessagePublished()
        {
            var eventPublisher = new EventPublisherFake();
            var owner = new UserId("owner@mixit.fr");
            var author = new UserId("author@mixit.fr");
            var content = "Hello";
            var messageId = MessageId.Generate();

            TimelineMessage.Publish(eventPublisher, owner, author, content, messageId);

            Check.That(eventPublisher.Events)
                 .Contains(new TimelineMessagePublished(new TimelineMessageId(owner, messageId), author, content));
        }
    }
}
