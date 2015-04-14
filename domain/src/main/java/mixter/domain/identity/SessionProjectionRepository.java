package mixter.domain.identity;

public interface SessionProjectionRepository {
    void save(SessionProjection sessionProjection);

    void replaceBy(SessionProjection sessionProjection);
}
