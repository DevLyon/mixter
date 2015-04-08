using System.Collections.Generic;
using System.Linq;
using Mixter.Domain.Core.Messages.Events;
using Mixter.Domain.Identity;

namespace Mixter.Domain.Core.Messages
{
    public class Message
    {
        private readonly DecisionProjection _projection = new DecisionProjection();

        private Message(IEventPublisher eventPublisher, MessagePublished evt)
        {
            PublishEvent(eventPublisher, evt);
        }

        public Message(IEnumerable<IDomainEvent> events)
        {
            foreach (var @event in events)
            {
                _projection.Apply(@event);
            }
        }

        public static Message Publish(IEventPublisher eventPublisher, UserId author, string content)
        {
            var messagePublished = new MessagePublished(MessageId.Generate(), author, content);
            return new Message(eventPublisher, messagePublished);
        }

        public void Republish(IEventPublisher eventPublisher, UserId republisher)
        {
            if (!_projection.Publishers.Contains(republisher) && !_projection.IsDeleted)
            {
                var evt = new MessageRepublished(GetId(), republisher);
                PublishEvent(eventPublisher, evt);
            }
        }

        private void PublishEvent<TEvent>(IEventPublisher eventPublisher, TEvent evt) where TEvent : IDomainEvent
        {
            eventPublisher.Publish(evt);
            _projection.Apply(evt);
        }

        public void Reply(IEventPublisher eventPublisher, UserId replier, string replyContent)
        {
            if (!_projection.IsDeleted)
            {
                var evt = new ReplyMessagePublished(MessageId.Generate(), replier, replyContent, _projection.Id);
                eventPublisher.Publish(evt);
            }
        }

        public void Delete(IEventPublisher eventPublisher, UserId deleter)
        {
            if (_projection.Author.Equals(deleter) && !_projection.IsDeleted)
            {
                PublishEvent(eventPublisher, new MessageDeleted(_projection.Id));
            }
        }

        public MessageId GetId()
        {
            return _projection.Id;
        }

        private class DecisionProjection : DecisionProjectionBase
        {
            private readonly IList<UserId> _publishers = new List<UserId>();

            public MessageId Id { get; private set; }

            public IEnumerable<UserId> Publishers
            {
                get { return _publishers; }
            }

            public bool IsDeleted { get; private set; }
            
            public UserId Author { get; private set; }

            public DecisionProjection()
            {
                AddHandler<MessagePublished>(When);
                AddHandler<MessageRepublished>(When);
                AddHandler<ReplyMessagePublished>(When);
                AddHandler<MessageDeleted>(When);
            }

            private void When(MessageDeleted evt)
            {
                IsDeleted = true;
            }

            private void When(ReplyMessagePublished evt)
            {
                Id = evt.ReplyId;
            }

            private void When(MessagePublished evt)
            {
                Id = evt.Id;
                Author = evt.Author;
                _publishers.Add(evt.Author);
            }

            private void When(MessageRepublished evt)
            {
                _publishers.Add(evt.Republisher);
            }
        }
    }
}
