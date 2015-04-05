using Mixter.Domain.Core.Messages.Events;
using Mixter.Domain.Identity;

namespace Mixter.Domain.Core.Messages
{
    public struct MessageDescription
    {
        public UserId Author { get; private set; }

        public string Content { get; private set; }

        public MessageDescription(MessagePublished evt)
            : this(evt.Author, evt.Content)
        {
        }

        public MessageDescription(ReplyMessagePublished evt)
            : this(evt.Replier, evt.ReplyContent)
        {
        }

        public MessageDescription(UserId author, string content)
            : this()
        {
            Author = author;
            Content = content;
        }
    }
}
