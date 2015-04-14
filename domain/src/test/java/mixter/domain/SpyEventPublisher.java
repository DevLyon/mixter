package mixter.domain;

import mixter.domain.core.message.events.MessageQuacked;
import org.assertj.core.util.Lists;

import java.util.List;

public class SpyEventPublisher implements EventPublisher{
    public List<MessageQuacked> publishedEvents= Lists.newArrayList();

    @Override
    public void publish(MessageQuacked messageQuacked) {
        publishedEvents.add(messageQuacked);
    }
}
