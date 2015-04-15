package mixter.domain.core.message.handlers;

import mixter.doc.Handler;
import mixter.domain.core.message.TimelineMessageProjection;
import mixter.domain.core.message.TimelineMessageRepository;
import mixter.domain.core.message.events.MessagePublished;
import mixter.domain.core.message.events.ReplyMessagePublished;

@Handler
public class UpdateTimeline {
    private TimelineMessageRepository timelineRepository;

    public UpdateTimeline(TimelineMessageRepository timelineRepository) {

        this.timelineRepository = timelineRepository;
    }

    public void apply(MessagePublished event) {
        timelineRepository.save(new TimelineMessageProjection(event.getAuthorId(), event.getAuthorId(), event.getMessage(), event.getMessageId()));
    }

    public void apply(ReplyMessagePublished event) {
        timelineRepository.save(new TimelineMessageProjection(event.getReplierId(), event.getReplierId(), event.getMessage(), event.getMessageId()));
    }
}
