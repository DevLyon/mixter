package mixter.infra;

import org.junit.Before;
import org.junit.Test;

import java.util.function.Consumer;

import static org.assertj.core.api.Assertions.assertThat;

public class SynchronousEventPublisherTest {
    private SpyEvenHandler<EventA> spyEvenHandler;

    @Before
    public void setUp() throws Exception {
        spyEvenHandler = new SpyEvenHandler<>();
    }

    @Test
    public void GivenASynchronousEventPublisherWithARegisteredHandlerWhenAnEventIsPublishedToItThenTheHandlerIsCalled() {
        Consumer<EventA> handler = spyEvenHandler::apply;
        SynchronousEventPublisher publisher = new SynchronousEventPublisher();
        publisher.register(EventA.class, handler);
        EventA event = new EventA(new AnAggregateId());

        publisher.publish(event);

        assertThat(spyEvenHandler.isCalled()).isTrue();
    }

    @Test
    public void GivenASynchronousEventPublisherWith2RegisteredHandlersWhenAnEventIsPublishedToItThenBothHandlersAreCalled() {
        Consumer<EventA> handler = spyEvenHandler::apply;
        SpyEvenHandler<EventA> spyEvenHandler2 = new SpyEvenHandler<>();
        Consumer<EventA> handler2 = spyEvenHandler2::apply;
        SynchronousEventPublisher publisher = new SynchronousEventPublisher();
        publisher.register(EventA.class, handler);
        publisher.register(EventA.class, handler2);
        EventA event = new EventA(new AnAggregateId());

        publisher.publish(event);

        assertThat(this.spyEvenHandler.isCalled()).isTrue();
        assertThat(spyEvenHandler2.isCalled()).isTrue();
    }

    @Test
    public void GivenASynchronousEventPublisherWith2RegisteredHandlersForDifferentEventsWhenAnEventIsPublishedToItThenOnlyTheRightHandlerIsCalled() {
        Consumer<EventA> handlerOfA = spyEvenHandler::apply;
        SpyEvenHandler<EventB> spyEvenHandler2 = new SpyEvenHandler<>();
        Consumer<EventB> handlerOfB = spyEvenHandler2::apply;
        SynchronousEventPublisher publisher = new SynchronousEventPublisher();
        publisher.register(EventA.class, handlerOfA);
        publisher.register(EventB.class, handlerOfB);
        EventA event = new EventA(new AnAggregateId());

        publisher.publish(event);

        assertThat(spyEvenHandler.isCalled()).isTrue();
        assertThat(spyEvenHandler2.isCalled()).isFalse();
    }
}
