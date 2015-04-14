package mixter.domain.identity;

import org.junit.Test;

import static org.assertj.core.api.Assertions.assertThat;

public class UserIdTest {

    public static final String AN_EMAIL = "mail@mix-it.fr";

    @Test
    public void GivenAnEmailWhenCreatingAUserIdThenAUserIdShouldBeCreate() throws Exception {
        UserId userId1 = new UserId(AN_EMAIL);
    }
    @Test
    public void Given2UserIdCreatedFromTheSameEmailWhenComparedWithEqualsThenTheyShouldBeEqual() throws Exception {
        UserId userId1 = new UserId(AN_EMAIL);
        UserId userId2 = new UserId(AN_EMAIL);

        assertThat(userId1).isEqualTo(userId2);
    }

    @Test
    public void GivenAnExistingUserIdWhenSerializingItToStringThenItShouldSerializeAsTheEmailString() throws Exception {
        //Given
        UserId userId1 = new UserId(AN_EMAIL);
        //When
        String seralizedUserId = userId1.toString();
        //Then
        assertThat(seralizedUserId).isEqualTo(AN_EMAIL);
    }
}
