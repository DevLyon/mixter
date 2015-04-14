package mixter.domain.identity;

import org.junit.Rule;
import org.junit.Test;
import org.junit.rules.ExpectedException;

import static org.assertj.core.api.Assertions.assertThat;

public class SessionIdTest {

    public static final String VALUE = "Value";

    @Test
    public void Given2GeneratedSessionIdsWhenComparedWithEqualsThenTheyShouldNotBeEqual() throws Exception {
        SessionId sessionId = SessionId.generate();
        SessionId otherSessionId = SessionId.generate();
        assertThat(sessionId).isNotEqualTo(otherSessionId);
    }

    @Test
    public void Given2SessionIdCreatedFromTheSameStringWhenComparedWithEqualsThenTheyShouldBeEqual() throws Exception {
        SessionId sessionId = new SessionId(VALUE);
        SessionId otherSessionId = new SessionId(VALUE);

        assertThat(sessionId).isEqualTo(otherSessionId);
    }

    @Rule
    public ExpectedException thrown = ExpectedException.none();

    @Test
    public void GivenAnEmptyStringWhenCreatingASessionIdThenAnExceptionShouldBeThrown() throws Exception {
        //Given
        String value = "";
        thrown.expect(SessionIdCannotBeEmpty.class);
        //When
        new SessionId(value);
    }
}
