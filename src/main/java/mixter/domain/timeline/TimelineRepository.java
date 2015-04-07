package mixter.domain.timeline;

interface TimelineRepository {
    TimelineMessage save(TimelineMessage message);
}
