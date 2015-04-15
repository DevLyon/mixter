package mixter.infra.repositories;

import mixter.domain.identity.UserId;
import mixter.domain.identity.UserIdentity;
import mixter.infra.EventStore;
import mixter.infra.InMemoryEventStore;
import org.junit.Before;
import org.junit.Rule;
import org.junit.Test;
import org.junit.rules.ExpectedException;

import java.util.NoSuchElementException;

public class EventUserIdentityRepositoryTest {
    public static final UserId USER_ID = new UserId("user@mix-it.fr");
    private EventStore eventStore;

    @Before
    public void setUp() throws Exception {
        eventStore = new InMemoryEventStore();
    }

    @Rule
    public ExpectedException thrown = ExpectedException.none();

    @Test
    public void shouldNotFindUserIdentityIfNoEventForUserId() throws Exception {
        //Given
        EventUserIdentityRepository repository = new EventUserIdentityRepository(eventStore);
        thrown.expect(NoSuchElementException.class);
        //When
        UserIdentity userIdentity = repository.getById(USER_ID);
    }
}
