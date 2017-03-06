package mixter.infra.repositories

import mixter.domain.identity.event.{UserConnected, UserSessionEvent}
import mixter.domain.identity.{Session, SessionId}
import mixter.infra.EventStore

class EventSessionRepository(store: EventStore) {
  def getById(id: SessionId):Option[Session] ={
    val history = store.eventsOfAggregate(id)
    history.headOption.map( userConnected=>
      Session(userConnected.asInstanceOf[UserConnected], history.tail.map(_.asInstanceOf[UserSessionEvent]))
    )
  }
}
