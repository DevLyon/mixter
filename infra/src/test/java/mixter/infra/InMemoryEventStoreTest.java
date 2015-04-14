package mixter.infra;

import mixter.domain.AggregateId;
import mixter.domain.Event;
import org.junit.Before;
import org.junit.Test;

import java.util.List;

import static org.assertj.core.api.Assertions.assertThat;

public class InMemoryEventStoreTest {
    public static final AnAggregateId AGGREGATE_ID1 = new AnAggregateId();

    InMemoryEventStore store = new InMemoryEventStore();

    @Before
    public void setUp() throws Exception {
        store = new InMemoryEventStore();

    }

    @Test
    public void GivenAnEmptyEventStoreWhenGettingEventsOfAggregateThenItReturnsAnEmptyList() {
        // Given
        AggregateId aggregateId1 = AGGREGATE_ID1;

        // When
        List<Event> eventsOfAggregate = store.getEventsOfAggregate(aggregateId1);

        // Then
        assertThat(eventsOfAggregate).isEmpty();
    }
}
