namespace Mixter.Domain
{
    public class Message
    {
        private readonly DecisionProjection _projection;

        private Message(MessagePublished evt)
        {
            _projection = new DecisionProjection(evt);
        }

        public static Message PublishMessage(IEventPublisher eventPublisher, string content)
        {
            var messagePublished = new MessagePublished(MessageId.Generate(), content);
            eventPublisher.Publish(messagePublished);

            return new Message(messagePublished);
        }

        public void RepublishMessage(IEventPublisher eventPublisher)
        {
            eventPublisher.Publish(new MessageRepublished(GetId()));
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

            private void Apply(MessagePublished evt)
            {
                Id = evt.Id;
            }
        }
    }
}
