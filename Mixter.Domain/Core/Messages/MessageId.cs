using System;

namespace Mixter.Domain.Core.Messages
{
    public struct MessageId
    {
        public MessageId(string id)
            : this()
        {
            Id = id;
        }

        public string Id { get; private set; }

        public static MessageId Generate()
        {
            return new MessageId(Guid.NewGuid().ToString());
        }

        public override string ToString()
        {
            return Id;
        }
    }
}
