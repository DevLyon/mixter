package mixter.infra.repositories

import mixter.domain.identity.Session
import mixter.infra.EventStore

class EventSessionRepository(override val store: EventStore) extends AggregateRepository[Session]{
  override protected def build(initialEvent: Session#InitialEvent, history: Seq[Session#AggregateEvent]): Session =
      Session(initialEvent, history)
}
