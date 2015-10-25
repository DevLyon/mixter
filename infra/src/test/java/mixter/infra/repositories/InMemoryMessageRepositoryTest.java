package mixter.infra.repositories;

import mixter.domain.core.message.Message;
import mixter.domain.core.message.MessageId;
import mixter.domain.core.message.events.MessageQuacked;
import mixter.domain.identity.UserId;
import mixter.infra.EventStore;
import mixter.infra.InMemoryEventStore;
import org.junit.Before;
import org.junit.Rule;
import org.junit.Test;
import org.junit.rules.ExpectedException;

import java.util.NoSuchElementException;

import static org.assertj.core.api.Assertions.assertThat;

public class InMemoryMessageRepositoryTest {
    private static final UserId AUTHOR_ID = new UserId("alice@mix.it");
    public static final MessageId MESSAGE_ID = MessageId.generate();
    private EventStore eventStore;
    private InMemoryMessageRepository repository;

    @Before
    public void setUp() throws Exception {
        eventStore=new InMemoryEventStore();
        repository=new InMemoryMessageRepository(eventStore);
    }

    @Test
    public void givenARepositoryWithSavedMessageWhenGettingTheMessageByIdThenTheMessageIsReturned() throws Exception {
        eventStore.store(new MessageQuacked(MESSAGE_ID,"Aloa",AUTHOR_ID));

        Message message = repository.getById(MESSAGE_ID);

        assertThat(message).isNotNull();
    }


    @Rule
    public ExpectedException thrown = ExpectedException.none();


    @Test
    public void givenNoEventsWhenGetSubscriptionThenThrowUnknownSubscriptionException() throws Exception {
        thrown.expect(NoSuchElementException.class);

        Message message = repository.getById(MESSAGE_ID);
    }
}