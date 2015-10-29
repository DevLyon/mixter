package mixter.domain.core.message;

import mixter.doc.Repository;
import mixter.domain.identity.UserId;
import sun.reflect.generics.reflectiveObjects.NotImplementedException;

import java.util.Iterator;

@Repository
public interface TimelineMessageRepository {
    TimelineMessageProjection save(TimelineMessageProjection message);

    Iterator<TimelineMessageProjection> getMessageOfUser(UserId ownerId);

    //this is done only for the exercise to avoid git conflitcts when
    // moving from step to step, this should not be done in real code
    default void delete(MessageId messageId){
        throw new NotImplementedException();
    }
}
