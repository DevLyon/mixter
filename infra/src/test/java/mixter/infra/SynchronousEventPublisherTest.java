package mixter.infra;

import org.junit.Before;
import org.junit.Test;

import java.util.function.Consumer;

import static org.assertj.core.api.Assertions.assertThat;

public class SynchronousEventPublisherTest {
    private SpyEvenHandler spyEvenHandler;

    @Before
    public void setUp() throws Exception {
        spyEvenHandler = new SpyEvenHandler();
    }

    @Test
    public void GivenASyncrhonousEventPublisherWithARegisteredHandlerWhenAnEventIsPublishedToItThenTheHandlerIsCalled() {
        Consumer<EventA> handler = spyEvenHandler::apply;
        SynchronousEventPublisher publisher = new SynchronousEventPublisher();
        publisher.register(EventA.class, handler);
        EventA event = new EventA(new AnAggregateId());

        publisher.publish(event);

        assertThat(spyEvenHandler.isCalled()).isTrue();
    }

}
