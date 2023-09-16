﻿using Mixter.Domain.Core.Messages.Events;

namespace Mixter.Domain.Core.Messages.Handlers
{
    [Handler]
    public class UpdateTimeline : 
        IEventHandler<MessageQuacked>
    {
        private readonly ITimelineMessageRepository _repository;

        public UpdateTimeline(ITimelineMessageRepository repository)
        {
            _repository = repository;
        }

        public void Handle(MessageQuacked evt)
        {
            var projection = new TimelineMessageProjection(evt.Author, evt.Author, evt.Content, evt.Id);
            _repository.Save(projection);
        }

        public void Handle(MessageDeleted evt)
        {
            _repository.Delete(evt.MessageId);
        }
    }
}
