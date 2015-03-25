namespace Mixter.Domain
{
    public class Message
    {
        private readonly MessageDecisionProjection _projection;

        private Message(MessagePublished evt)
        {
            _projection = new MessageDecisionProjection(evt);
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
    }
}
