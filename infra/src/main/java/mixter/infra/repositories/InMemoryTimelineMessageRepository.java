package mixter.infra.repositories;

import mixter.domain.core.message.TimelineMessageProjection;
import mixter.domain.core.message.TimelineMessageRepository;
import mixter.domain.identity.UserId;

import java.util.*;

public class InMemoryTimelineMessageRepository implements TimelineMessageRepository {
    private Set<TimelineMessageProjection> messages = new HashSet<>();
    @Override
    public TimelineMessageProjection save(TimelineMessageProjection message) {
        messages.add(message);
        return message;
    }

    @Override
    public Iterator<TimelineMessageProjection> getMessageOfUser(UserId ownerId) {
        return messages.stream().filter(m->ownerId.equals(m.getOwnerId())).iterator();
    }
}
