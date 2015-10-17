using Mixter.Domain.Core.Messages.Events;
using Mixter.Domain.Identity;

namespace Mixter.Domain.Core.Messages
{
    [Projection]
    public struct MessageDescription
    {
        public UserId Author { get; private set; }

        public string Content { get; private set; }

        public MessageDescription(MessageQuacked evt)
            : this(evt.Author, evt.Content)
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