using System;
using Mixter.Domain;
using Mixter.Domain.Core.Messages.Events;

namespace Mixter.Web
{
    public class MessagePublishedHandler : IEventHandler<MessagePublished>
    {
        public void Handle(MessagePublished evt)
        {
            throw new NotImplementedException();
        }
    }
}
