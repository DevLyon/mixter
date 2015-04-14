package mixter.infra;

import mixter.domain.AggregateId;
import mixter.domain.Event;
import org.junit.Before;
import org.junit.Test;

import java.util.List;

import static org.assertj.core.api.Assertions.assertThat;

public class InMemoryEventStoreTest {
    public static final AnAggregateId AGGREGATE_ID1 = new AnAggregateId();
    public static final AnAggregateId AGGREGATE_ID2 = new AnAggregateId();
    public static final AnAggregateId AGGREGATE_ID3 = new AnAggregateId();

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

    @Test
    public void GivenAnEmptyEventStoreWhenStoringAnEventAndGettingTheEventsOfTheAggregateThenItReturnsTheEvent() {
        // Given
        AggregateId aggregateId1 = AGGREGATE_ID1;
        EventA expected = new EventA(aggregateId1);

        // When
        store.store(expected);
        List<Event> eventsOfAggregate = store.getEventsOfAggregate(aggregateId1);

        // Then
        assertThat(eventsOfAggregate).containsExactly(expected);
    }

    @Test
    public void GivenAStoreHavingStoredEventsOfSeveralAggregatesWhenGetEventsOfOneAggregateThenReturnEventsOfOnlyThisAggregate() {
        EventA expected = new EventA(AGGREGATE_ID1);
        store.store(expected);
        store.store(new EventA(AGGREGATE_ID2));
        store.store(new EventA(AGGREGATE_ID3));

        List<Event> eventsOfAggregateA = store.getEventsOfAggregate(AGGREGATE_ID1);

        assertThat(eventsOfAggregateA).containsExactly(expected);
    }
}
