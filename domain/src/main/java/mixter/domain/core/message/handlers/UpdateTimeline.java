package mixter.domain.core.message.handlers;

import mixter.doc.Handler;
import mixter.domain.core.message.TimelineMessageProjection;
import mixter.domain.core.message.TimelineMessageRepository;
import mixter.domain.core.message.events.MessageDeleted;
import mixter.domain.core.message.events.MessageQuacked;

@Handler
public class UpdateTimeline {
    private TimelineMessageRepository timelineRepository;

    public UpdateTimeline(TimelineMessageRepository timelineRepository) {

        this.timelineRepository = timelineRepository;
    }

    public void apply(MessageQuacked event) {
        timelineRepository.save(new TimelineMessageProjection(event.getAuthorId(), event.getAuthorId(), event.getMessage(), event.getMessageId()));
    }

    public void apply(MessageDeleted messageDeleted) {
        timelineRepository.delete(messageDeleted.getId());
    }
}
