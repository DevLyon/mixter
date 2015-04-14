package mixter.infra.repositories;

import mixter.domain.identity.SessionId;
import mixter.domain.identity.UserId;
import org.junit.Test;

import java.util.Optional;

import static org.assertj.core.api.Assertions.assertThat;

public class InMemorySessionProjectionRepositoryTest {
    public static final UserId USER_ID = new UserId("mail@mix-it.fr");

    @Test
    public void GivenAnEmptyRepositoryWhenRetrievingByIdThenItShouldHaveNoProjection() throws Exception {
        InMemorySessionProjectionRepository repository = new InMemorySessionProjectionRepository();

        assertThat(repository.getById(SessionId.generate())).isEqualTo(Optional.empty());
    }

}
