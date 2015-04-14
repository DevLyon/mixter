package mixter.domain.identity;

import mixter.domain.DomainTest;
import mixter.domain.Event;
import mixter.domain.SpyEventPublisher;
import mixter.domain.identity.events.UserConnected;
import mixter.domain.identity.events.UserDisconnected;
import org.junit.Before;
import org.junit.Test;

import java.time.Instant;

import static org.assertj.core.api.Assertions.assertThat;

public class SessionTest extends DomainTest {

    private SpyEventPublisher eventPublisher;

    private SessionId SESSION_ID = SessionId.generate();
    private UserId USER_ID = new UserId("user@mixit.fr");

    @Before
    public void setUp() throws Exception {
        eventPublisher = new SpyEventPublisher();
    }

    @Test
    public void GivenAnExistingSessionWhenLoggingOutThenUserDisconnectedIsRaised() {
        // Given
        Session session = sessionFor(new UserConnected(SESSION_ID, USER_ID, Instant.now()));

        // When
        session.logout(eventPublisher);

        // Then
        UserDisconnected expected = new UserDisconnected(SESSION_ID, USER_ID);
        assertThat(eventPublisher.publishedEvents).containsExactly(expected);
    }

    @Test
    public void GivenADisconnectedSessionWhenLoggingOutThenNoEventIsRaised() {
        // Given
        Session session = sessionFor(
                new UserConnected(SESSION_ID, USER_ID, Instant.now()),
                new UserDisconnected(SESSION_ID, USER_ID));

        // When
        session.logout(eventPublisher);

        // Then
        assertThat(eventPublisher.publishedEvents).isEmpty();
    }

    private Session sessionFor(Event... events) {
        return new Session(history(events));
    }
}
