package mixter.infra

import mixter.domain.{Event, EventPublisher}

class SpyEventPublisher extends EventPublisher {
  var publishedEvents:Seq[Event]=Seq.empty
  override def publish[T<:Event](event:T): Unit = {
    publishedEvents = publishedEvents :+ event
  }
}
