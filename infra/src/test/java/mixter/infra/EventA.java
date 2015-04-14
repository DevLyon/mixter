package mixter.infra;

import mixter.domain.AggregateId;
import mixter.domain.Event;

public class EventA implements Event {
    private AggregateId aggregateId;

    public EventA(AggregateId aggregateId) {
        this.aggregateId = aggregateId;
    }

    @Override
    public AggregateId getId() {
        return aggregateId;
    }
}
