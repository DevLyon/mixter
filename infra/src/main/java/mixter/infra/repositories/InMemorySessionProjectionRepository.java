package mixter.infra.repositories;

import mixter.domain.identity.SessionId;
import mixter.domain.identity.SessionProjection;
import mixter.domain.identity.SessionProjectionRepository;

import java.util.HashMap;
import java.util.Map;
import java.util.Optional;

public class InMemorySessionProjectionRepository implements SessionProjectionRepository {
    Map<SessionId, SessionProjection> sessions = new HashMap<>();
    @Override
    public void save(SessionProjection sessionProjection) {
        sessions.put(sessionProjection.getSessionId(), sessionProjection);
    }

    @Override
    public void replaceBy(SessionProjection sessionProjection) {
        sessions.put(sessionProjection.getSessionId(), sessionProjection);
    }

    @Override
    public Optional<SessionProjection> getById(SessionId sessionId) {
        return Optional.ofNullable(sessions.get(sessionId));
    }


}
