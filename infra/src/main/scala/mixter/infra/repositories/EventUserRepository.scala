package mixter.infra.repositories

import mixter.domain.identity.UserIdentity
import mixter.infra.EventStore

class EventUserRepository(override val store: EventStore) extends AggregateRepository[UserIdentity]{
  override protected def build(initialEvent: UserIdentity#InitialEvent, history: Seq[UserIdentity#AggregateEvent]): UserIdentity =
      UserIdentity(initialEvent, history)

}
