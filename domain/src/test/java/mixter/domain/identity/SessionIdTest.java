package mixter.domain.identity;

import org.junit.Test;

import static org.assertj.core.api.Assertions.assertThat;

public class SessionIdTest {
    @Test
    public void Given2GeneratedSessionIdsWhenComparedWithEqualsThenTheyShouldNotBeEqual() throws Exception {
        SessionId sessionId = SessionId.generate();
        SessionId otherSessionId = SessionId.generate();
        assertThat(sessionId).isNotEqualTo(otherSessionId);
    }
}
