package domain

import domain.message.event.MessageQuacked

class SpyEventPublisher extends EventPublisher {
  var publishedEvents:Seq[MessageQuacked]=Seq.empty
  override def publish(messageQuacked:MessageQuacked): Unit = {
    publishedEvents = publishedEvents :+ messageQuacked
  }
}
