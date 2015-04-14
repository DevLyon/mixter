package mixter.infra;

import org.junit.Before;
import org.junit.Test;

import static org.assertj.core.api.Assertions.assertThat;

public class PersistingEventPublisherTest {

    private InMemoryEventStore store;
    private SpyEventPublisher publisher;

    @Before
    public void setUp() throws Exception {
        store = new InMemoryEventStore();
        publisher = new SpyEventPublisher();
    }

    @Test
    public void GivenAPersistingEventPublisherWhenPublishingAnEventThenItIsStored() throws Exception {
        // Given
        AnAggregateId id = new AnAggregateId();
        PersistingEventPublisher persistedPublisher = new PersistingEventPublisher(store, publisher);
        EventA expected = new EventA(id);
        // When
        persistedPublisher.publish(expected);

        // Then
        assertThat(store.getEventsOfAggregate(id)).containsExactly(expected);
    }

    @Test
    public void GivenAPersistingEventPublisherWhenPublishingAnEventThenItIsForwarded() throws Exception {
        // Given
        AnAggregateId id = new AnAggregateId();
        PersistingEventPublisher persistedPublisher = new PersistingEventPublisher(store, publisher);
        EventA expected = new EventA(id);
        // When
        persistedPublisher.publish(expected);

        // Then
        assertThat(publisher.publishedEvents).containsExactly(expected);
    }
}
