package mixter.domain.identity;

import mixter.doc.Repository;

@Repository
public interface SessionProjectionRepository {
    void save(SessionProjection sessionProjection);

    void replaceBy(SessionProjection sessionProjection);
}
