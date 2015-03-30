using Mixter.Domain;
using Mixter.Domain.Core.Messages;
using Mixter.Domain.Core.Messages.Events;
using Mixter.Domain.Identity;

namespace Mixter.Tests.Domain.Core.Messages
{
    public class TimelineMessage
    {
        public static void Publish(IEventPublisher eventPublisher, UserId owner, UserId author, string content, MessageId messageId)
        {
            var id = new TimelineMessageId(owner, messageId);
            var evt = new TimelineMessagePublished(id, author, content);
            eventPublisher.Publish(evt);
        }
    }
}