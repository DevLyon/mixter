package mixter.infra.repositories

import mixter.domain.identity.{UserId, UserIdentity}
import mixter.infra.EventStore

class EventUserRepository(store: EventStore) {
  def getById(id: UserId):Option[UserIdentity] ={
    None
  }
}
