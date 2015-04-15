package mixter.infra.repositories;

import mixter.domain.Event;
import mixter.domain.identity.UserIdentity;
import mixter.infra.EventStore;

import java.util.List;

public class EventUserIdentityRepository extends AggregateRepository<UserIdentity> {

    public EventUserIdentityRepository(EventStore eventStore) {
        super(eventStore);
    }

    @Override
    protected UserIdentity fromHistory(List<Event> history) {
        return new UserIdentity(history);
    }

}
