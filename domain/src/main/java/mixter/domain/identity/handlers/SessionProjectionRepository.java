package mixter.domain.identity.handlers;

import mixter.domain.identity.SessionProjection;

public interface SessionProjectionRepository {
    void save(SessionProjection sessionProjection);

    void replaceBy(SessionProjection sessionProjection);
}
