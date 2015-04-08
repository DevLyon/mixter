package mixter.domain.message.handlers;

import mixter.domain.message.TimelineMessage;
import mixter.domain.message.TimelineRepository;
import mixter.domain.message.events.MessagePublished;
import mixter.domain.message.events.MessageRepublished;
import mixter.domain.subscription.events.FolloweeMessagePublished;

public class UpdateTimeline {

    private final TimelineRepository repository;

    public UpdateTimeline(TimelineRepository repository) {
        this.repository = repository;
    }

    public void apply(MessagePublished message) {
        repository.save(new TimelineMessage(message.getAuthorId(), message.getAuthorId(), message.getMessage(), message.getMessageId()));
    }

    public void apply(MessageRepublished message) {
        repository.save(new TimelineMessage(message.getUserId(), message.getAuthorId(), message.getMessage(), message.getMessageId()));
    }

    public void apply(FolloweeMessagePublished messagePublished) {
        TimelineMessage original = repository.getByMessageId(messagePublished.getMessageId());
        repository.save(new TimelineMessage(messagePublished.getSubscriptionId().getFollower(), original.getAuthorId(), original.getContent(), original.getMessageId()));
    }
}
