package mixter.infra.repositories;

import mixter.domain.identity.UserId;
import mixter.domain.identity.UserIdentity;
import mixter.infra.EventStore;

import java.util.NoSuchElementException;

public class EventUserIdentityRepository {
    public EventUserIdentityRepository(EventStore eventStore) {

    }

    public UserIdentity getById(UserId userId) {
        throw new NoSuchElementException();
    }
}
