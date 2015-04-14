package mixter.infra;

import mixter.domain.AggregateId;
import mixter.domain.Event;

public class EventB implements Event {
    private AggregateId aggregateId;

    public EventB(AggregateId aggregateId) {
        this.aggregateId = aggregateId;
    }

    @Override
    public AggregateId getId() {
        return aggregateId;
    }
}
