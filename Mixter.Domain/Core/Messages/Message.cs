using Mixter.Domain.Core.Messages.Events;
using Mixter.Domain.Identity;

namespace Mixter.Domain.Core.Messages
{
    public class Message
    {
        public static MessageId Quack(IEventPublisher eventPublisher, UserId author, string content)
        {
            var messageId = MessageId.Generate();
            eventPublisher.Publish(new MessageQuacked(messageId, author, content));
            return messageId;
        }
    }
}
