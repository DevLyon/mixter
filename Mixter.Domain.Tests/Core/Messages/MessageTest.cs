using System.Linq;
using Mixter.Domain.Core.Messages;
using Mixter.Domain.Core.Messages.Events;
using Mixter.Domain.Identity;
using NFluent;
using Xunit;

namespace Mixter.Domain.Tests.Core.Messages
{
    public class MessageTest
    {
        private const string MessageContent = "Hello miixit";

        private static readonly UserId Author = new UserId("pierre@mixit.fr");

        private readonly EventPublisherFake _eventPublisher;

        public MessageTest()
        {
            _eventPublisher = new EventPublisherFake();
        }

        [Fact]
        public void WhenQuackMessageThenRaiseUserMessageQuacked()
        {
            Message.Quack(_eventPublisher, Author, MessageContent);

            var evt = (MessageQuacked)_eventPublisher.Events.First();
            Check.That(evt.Content).IsEqualTo(MessageContent);
            Check.That(evt.Author).IsEqualTo(Author);
        }

        [Fact]
        public void WhenQuackSeveralMessageThenMessageIdIsNotSame()
        {
            Message.Quack(_eventPublisher, Author, MessageContent);
            Message.Quack(_eventPublisher, Author, MessageContent);

            var events = _eventPublisher.Events.OfType<MessageQuacked>().ToArray();
            Check.That(events[0].Id).IsNotEqualTo(events[1].Id);
        }

        [Fact]
        public void WhenQuackMessageThenReturnMessageId()
        {
            var messageId = Message.Quack(_eventPublisher, Author, MessageContent);

            var evt = (MessageQuacked)_eventPublisher.Events.First();
            Check.That(evt.Id).IsEqualTo(messageId);
        }
    }
}
