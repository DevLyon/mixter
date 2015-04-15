package mixter.domain.core.message;

public interface TimelineMessageRepository {
    TimelineMessageProjection save(TimelineMessageProjection message);
}
