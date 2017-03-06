package mixter.infra.repositories

import mixter.domain.identity.SessionId
import mixter.infra.EventStore

class EventSessionRepository(store: EventStore) {
  def getById(id: SessionId) = None
}
