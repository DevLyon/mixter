package mixter.domain

class SpyEventPublisher extends EventPublisher {
  var publishedEvents:Seq[Event]=Seq.empty
  override def publish(event:Event): Unit = {
    publishedEvents = publishedEvents :+ event
  }
}
