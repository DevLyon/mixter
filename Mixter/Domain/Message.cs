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
            if (!_projection.Creator.Equals(republisher))
            {
                eventPublisher.Publish(new MessageRepublished(GetId(), republisher));
            }
        }

        public MessageId GetId()
        {
            return _projection.Id;
        }

        private class DecisionProjection
        {
            public DecisionProjection(MessagePublished evt)
            {
                Apply(evt);
            }

            public MessageId Id { get; private set; }

            public UserId Creator { get; private set; }

            private void Apply(MessagePublished evt)
            {
                Id = evt.Id;
                Creator = evt.UserId;
            }
        }
    }
}
