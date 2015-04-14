package mixter.infra.repositories;

import mixter.domain.identity.SessionId;
import mixter.domain.identity.SessionProjection;
import mixter.domain.identity.SessionStatus;
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

    @Test
    public void GivenARepositoryWithASavedSessionWhenRetrievingBySessionIdThenItShouldReturnTheSessionProjection() throws Exception {
        // Given
        SessionProjection sessionProjection = new SessionProjection(SessionId.generate(), USER_ID, SessionStatus.CONNECTED);
        InMemorySessionProjectionRepository repository = new InMemorySessionProjectionRepository();
        // When
        repository.save(sessionProjection);
        // Then
        assertThat(repository.getById(sessionProjection.getSessionId()).get()).isEqualTo(sessionProjection);
    }

}
