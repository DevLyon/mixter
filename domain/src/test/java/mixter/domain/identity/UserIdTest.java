package mixter.domain.identity;

import org.junit.Test;

public class UserIdTest {

    public static final String AN_EMAIL = "mail@mix-it.fr";

    @Test
    public void GivenAnEmailWhenCreatingAUserIdThenAUserIdShouldBeCreate() throws Exception {
        UserId userId1 = new UserId(AN_EMAIL);
    }
}
