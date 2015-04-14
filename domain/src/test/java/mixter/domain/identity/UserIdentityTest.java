package mixter.domain.identity;

import mixter.domain.SpyEventPublisher;
import mixter.domain.identity.events.UserRegistered;
import org.junit.Before;
import org.junit.Test;

import static org.assertj.core.api.Assertions.assertThat;

public class UserIdentityTest {
    public static final UserId USER_ID = new UserId("user@mix-it.fr");
    private SpyEventPublisher eventPublisher;

    @Before
    public void setUp() throws Exception {
        eventPublisher = new SpyEventPublisher();
    }

    @Test
    public void GivenAUserIdWhenRegisteringAUserIdentityThenAUserRegisteredEventIsRaised() {
        // When
        UserIdentity.register(eventPublisher, USER_ID);

        // Then
        UserRegistered expected = new UserRegistered(USER_ID);
        assertThat(eventPublisher.publishedEvents).contains(expected);
    }
}
