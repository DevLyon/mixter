package mixter.infra.repositories

import mixter.domain.identity.event.{UserEvent, UserRegistered}
import mixter.domain.identity.{UserId, UserIdentity}
import mixter.infra.EventStore

class EventUserRepository(store: EventStore) {
  def getById(id: UserId):Option[UserIdentity] ={
    val history = store.eventsOfAggregate(id)
    history.headOption.map( userConnected=>
      UserIdentity(userConnected.asInstanceOf[UserRegistered], history.tail.map(_.asInstanceOf[UserEvent]))
    )
  }
}
