using System.Collections.Generic;
using System.Linq;
using Mixter.Domain.Core.Messages.Events;
using Mixter.Domain.Identity;

namespace Mixter.Domain.Core.Messages
{
    public class Message
    {
        private readonly DecisionProjection _projection = new DecisionProjection();

        public Message(IEnumerable<IDomainEvent> events)
        {
            foreach (var @event in events)
            {
                _projection.Apply(@event);
            }
        }

        public static MessageId Quack(IEventPublisher eventPublisher, UserId author, string content)
        {
            var messageId = MessageId.Generate();
            eventPublisher.Publish(new MessageQuacked(messageId, author, content));
            return messageId;
        }

        public void Requack(IEventPublisher eventPublisher, UserId requacker)
        {
            if (_projection.Quackers.Contains(requacker) || _projection.IsDeleted)
            {
                return;
            }

            eventPublisher.Publish(new MessageRequacked(_projection.Id, requacker));
        }

        public void Delete(IEventPublisher eventPublisher, UserId deleter)
        {
            if (_projection.Author.Equals(deleter) && !_projection.IsDeleted)
            {
                eventPublisher.Publish(new MessageDeleted(_projection.Id, deleter));
            }
        }

        private class DecisionProjection : DecisionProjectionBase
        {
            private readonly IList<UserId> _quackers = new List<UserId>();

            public MessageId Id { get; private set; }

            public UserId Author { get; private set; }

            public IEnumerable<UserId> Quackers
            {
                get { return _quackers; }
            }

            public bool IsDeleted { get; private set; }

            public DecisionProjection()
            {
                AddHandler<MessageQuacked>(When);
                AddHandler<MessageRequacked>(When);
                AddHandler<MessageDeleted>(When);
            }

            private void When(MessageQuacked evt)
            {
                Id = evt.Id;
                Author = evt.Author;
                _quackers.Add(evt.Author);
            }

            private void When(MessageRequacked evt)
            {
                _quackers.Add(evt.Requacker);
            }

            private void When(MessageDeleted evt)
            {
                IsDeleted = true;
            }
        }
    }
}
