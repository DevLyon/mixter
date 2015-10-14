using System;

namespace Mixter.Domain.Identity
{
    public struct SessionId
    {
        public string Id { get; private set; }

        public SessionId(string id)
            : this()
        {
            Id = id;
        }

        public static SessionId Generate()
        {
            return new SessionId(Guid.NewGuid().ToString());
        }
    }
}
