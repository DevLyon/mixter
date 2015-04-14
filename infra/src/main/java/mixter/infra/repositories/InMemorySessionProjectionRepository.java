package mixter.infra.repositories;

import mixter.domain.identity.SessionId;
import mixter.domain.identity.SessionProjection;
import mixter.domain.identity.SessionProjectionRepository;

import java.util.Optional;

public class InMemorySessionProjectionRepository implements SessionProjectionRepository {
    @Override
    public void save(SessionProjection sessionProjection) {

    }

    @Override
    public void replaceBy(SessionProjection sessionProjection) {

    }

    @Override
    public Optional<SessionProjection> getById(SessionId sessionId) {
        return Optional.empty();
    }


}
