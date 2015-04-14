package mixter.domain;

import mixter.domain.core.message.events.MessagePublished;
import org.assertj.core.util.Lists;

import java.util.List;

public class SpyEventPublisher implements EventPublisher{
    public List<MessagePublished> publishedEvents= Lists.newArrayList();

    @Override
    public void publish(MessagePublished messagePublished) {
        publishedEvents.add(messagePublished);
    }
}
