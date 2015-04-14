package mixter.domain.identity;

import org.assertj.core.util.Sets;

import java.util.Optional;
import java.util.Set;

public class FakeSessionProjectionRepository implements SessionProjectionRepository {

    private Set<SessionProjection> sessions = Sets.newHashSet();

    public Set<SessionProjection> getSessions() {
        return sessions;
    }

    @Override
    public void save(SessionProjection sessionProjection) {
        sessions.add(sessionProjection);
    }

    @Override
    public void replaceBy(SessionProjection sessionProjection) {
        sessions.removeIf(s -> s.getSessionId().equals(sessionProjection.getSessionId()));
        sessions.add(sessionProjection);
    }

    @Override
    public Optional<SessionProjection> getById(SessionId sessionId) {
        return Optional.empty();
    }
}
