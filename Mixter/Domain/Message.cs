using System.Collections.Generic;
using System.Linq;

namespace Mixter.Domain
{
    public class Message
    {
        private readonly DecisionProjection _projection;

        private Message(MessagePublished evt)
        {
            _projection = new DecisionProjection(evt);
        }

        public static Message PublishMessage(IEventPublisher eventPublisher, UserId creator, string content)
        {
            var messagePublished = new MessagePublished(MessageId.Generate(), creator, content);
            eventPublisher.Publish(messagePublished);

            return new Message(messagePublished);
        }

        public void RepublishMessage(IEventPublisher eventPublisher, UserId republisher)
        {
            if (!_projection.Publishers.Contains(republisher))
            {
                var evt = new MessageRepublished(GetId(), republisher);
                eventPublisher.Publish(evt);
                _projection.Apply(evt);
            }
        }

        public MessageId GetId()
        {
            return _projection.Id;
        }

        private class DecisionProjection
        {
            private readonly IList<UserId> _publishers = new List<UserId>();

            public DecisionProjection(MessagePublished evt)
            {
                Apply(evt);
            }

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
