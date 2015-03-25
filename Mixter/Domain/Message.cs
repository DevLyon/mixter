namespace Mixter.Domain
{
    public class Message
    {
        public static void PublishMessage(IEventPublisher eventPublisher, string content)
        {
            var messagePublished = new MessagePublished(MessageId.Generate(), content);
            eventPublisher.Publish(messagePublished);
        }
    }
}
