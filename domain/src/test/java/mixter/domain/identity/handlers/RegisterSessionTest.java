package mixter.domain.identity.handlers;

import mixter.domain.identity.SessionId;
import mixter.domain.identity.SessionProjection;
import mixter.domain.identity.SessionStatus;
import mixter.domain.identity.UserId;
import mixter.domain.identity.events.UserConnected;
import org.junit.Before;
import org.junit.Test;

import java.time.Instant;

import static org.assertj.core.api.Assertions.assertThat;

public class RegisterSessionTest {
    private FakeSessionProjectionRepository sessionRepository;

    @Before
    public void setUp() throws Exception {
        sessionRepository = new FakeSessionProjectionRepository();
    }

    @Test
    public void GivenARegisterSessionHandlerWhenItReceivesAUserConnectedEventThenItStoresAConnectedSessionProjection() {
        // Given
        UserConnected userConnected = new UserConnected(SessionId.generate(), new UserId("user@mixit.fr"), Instant.now());
        RegisterSession handler = new RegisterSession(sessionRepository);
        // When
        handler.apply(userConnected);
        // Then
        SessionProjection sessionProjection = new SessionProjection(userConnected.getSessionId(), userConnected.getUserId(), SessionStatus.CONNECTED);
        assertThat(sessionRepository.getSessions()).containsExactly(sessionProjection);
    }
}
