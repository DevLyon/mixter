package mixter.domain.core.message;

import mixter.doc.Repository;
import mixter.domain.identity.UserId;
import java.util.Iterator;

@Repository
public interface TimelineMessageRepository {
    TimelineMessageProjection save(TimelineMessageProjection message);

    Iterator<TimelineMessageProjection> getMessageOfUser(UserId ownerId);
}
