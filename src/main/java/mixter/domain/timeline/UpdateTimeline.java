package mixter.domain.timeline;

import mixter.domain.message.MessagePublished;

public class UpdateTimeline {

    private final TimelineRepository repository;

    public UpdateTimeline(TimelineRepository repository) {

        this.repository = repository;
    }

    public void apply(MessagePublished message) {
        repository.save(new TimelineMessage(message.getAuthorId(), message.getAuthorId(), message.getMessage(), message.getMessageId()));
    }
}
