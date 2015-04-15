package mixter.domain.core.message;

import mixter.doc.Repository;

@Repository
public interface TimelineMessageRepository {
    TimelineMessageProjection save(TimelineMessageProjection message);
}
