package mixter.infra

import mixter.domain.{Event, EventPublisher}

class PersistingEventPublisher(eventStore: EventStore, eventPublisher: EventPublisher) extends EventPublisher {

  override def publish[T <: Event](event: T): Unit = {
    eventStore.store(event)
  }
}
