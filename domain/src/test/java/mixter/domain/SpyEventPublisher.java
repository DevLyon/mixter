package mixter.domain;

import org.assertj.core.util.Lists;

import java.util.List;

public class SpyEventPublisher implements EventPublisher{
    public List<Event> publishedEvents= Lists.newArrayList();

    @Override
    public void publish(Event messagePublished) {
        publishedEvents.add(messagePublished);
    }
}
