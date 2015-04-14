package mixter.domain.identity;

import mixter.doc.Repository;

import java.util.Optional;

@Repository
public interface SessionProjectionRepository {
    void save(SessionProjection sessionProjection);

    void replaceBy(SessionProjection sessionProjection);

    Optional<SessionProjection> getById(SessionId sessionId);
}
