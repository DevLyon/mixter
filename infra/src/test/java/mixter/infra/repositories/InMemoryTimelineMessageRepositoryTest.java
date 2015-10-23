package mixter.infra.repositories;

import mixter.domain.core.message.MessageId;
import mixter.domain.core.message.TimelineMessageProjection;
import mixter.domain.core.message.TimelineMessageRepository;
import mixter.domain.identity.UserId;
import org.junit.Test;

import static org.assertj.core.api.Assertions.assertThat;

public class InMemoryTimelineMessageRepositoryTest {
    UserId OWNER_ID = new UserId("joe@mixit.fr");
    UserId AUTHOR_ID = new UserId("joe@mixit.fr");
    UserId ALICE_ID = new UserId("alice@mixit.fr");

    @Test
    public void givenAMessageSavedWhenGetMessagesOfUserThenMessageIsReturned() throws Exception {
        TimelineMessageRepository repository = new InMemoryTimelineMessageRepository();
        MessageId messageId = MessageId.generate();

        TimelineMessageProjection timelineProjection = new TimelineMessageProjection(OWNER_ID, AUTHOR_ID, "Hello", messageId);
        repository.save(timelineProjection);

        assertThat(repository.getMessageOfUser(OWNER_ID)).containsExactly(timelineProjection);
    }

    @Test
    public void givenTwoMessagesOfDifferentOwnerSavedWhenGetMessagesOfUserThenOnlyUserMessageIsReturned() throws Exception {
        TimelineMessageRepository repository = new InMemoryTimelineMessageRepository();
        MessageId messageId = MessageId.generate();

        TimelineMessageProjection joeMessage = new TimelineMessageProjection(OWNER_ID, AUTHOR_ID, "Hello", messageId);
        TimelineMessageProjection aliceMessage = new TimelineMessageProjection(ALICE_ID, AUTHOR_ID, "Hello", messageId);
        repository.save(joeMessage);
        repository.save(aliceMessage);

        assertThat(repository.getMessageOfUser(OWNER_ID)).containsExactly(joeMessage);
    }

    @Test
    public void givenTwoIdenticalMessagesSavedWhenGetMessagesOfUserThenOnlyOneMessageIsReturned() throws Exception {
        TimelineMessageRepository repository = new InMemoryTimelineMessageRepository();
        MessageId messageId = MessageId.generate();

        TimelineMessageProjection joeMessage = new TimelineMessageProjection(OWNER_ID, AUTHOR_ID, "Hello", messageId);
        repository.save(joeMessage);
        repository.save(joeMessage);

        assertThat(repository.getMessageOfUser(OWNER_ID)).containsOnlyOnce(joeMessage);
    }

    @Test
    public void giventAMessageSavedForSeveralUsersWhenRemoveThisMessageThenRemoveForAllUsers() throws Exception {
        TimelineMessageRepository repository = new InMemoryTimelineMessageRepository();
        MessageId messageId = MessageId.generate();

        TimelineMessageProjection joeMessage = new TimelineMessageProjection(OWNER_ID, AUTHOR_ID, "Hello", messageId);
        TimelineMessageProjection aliceMessage = new TimelineMessageProjection(ALICE_ID, AUTHOR_ID, "Hello", messageId);
        repository.save(joeMessage);
        repository.save(aliceMessage);

        repository.delete(messageId);

        assertThat(repository.getMessageOfUser(OWNER_ID)).isEmpty();
        assertThat(repository.getMessageOfUser(ALICE_ID)).isEmpty();
    }
}
