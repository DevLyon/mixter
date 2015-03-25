namespace Mixter.Domain
{
    public class Message
    {
        public static void PublishMessage(IEventPublisher eventPublisher, string message)
        {
            var messagePublished = new MessagePublished(MessageId.Generate(), message);
            eventPublisher.Publish(messagePublished);
        }
    }
}
