using System.Collections.Generic;
using System.Linq;

namespace Mixter.Domain
{
    public class Message
    {
        private readonly DecisionProjection _projection = new DecisionProjection();

        private Message(IEventPublisher eventPublisher, MessagePublished evt)
        {
            PublishEvent(eventPublisher, evt);
        }

        public static Message PublishMessage(IEventPublisher eventPublisher, UserId creator, string content)
        {
            var messagePublished = new MessagePublished(MessageId.Generate(), creator, content);
            return new Message(eventPublisher, messagePublished);
        }

        public void RepublishMessage(IEventPublisher eventPublisher, UserId republisher)
        {
            if (!_projection.Publishers.Contains(republisher))
            {
                var evt = new MessageRepublished(GetId(), republisher);
                PublishEvent(eventPublisher, evt);
            }
        }

        private void PublishEvent(IEventPublisher eventPublisher, IDomainEvent evt)
        {
            eventPublisher.Publish(evt);
            _projection.Apply((dynamic)evt);
        }

        public MessageId GetId()
        {
            return _projection.Id;
        }

        private class DecisionProjection
        {
            private readonly IList<UserId> _publishers = new List<UserId>();

            public MessageId Id { get; private set; }

            public IEnumerable<UserId> Publishers
            {
                get { return _publishers; }
            }

            public void Apply(MessagePublished evt)
            {
                Id = evt.Id;
                _publishers.Add(evt.Creator);
            }

            public void Apply(MessageRepublished evt)
            {
                _publishers.Add(evt.Republisher);
            }
        }
    }
}
