package mixter.domain

class SpyEventPublisher extends EventPublisher {
  var publishedEvents:Seq[Event]=Seq.empty
  override def publish[T<:Event](event:T): Unit = {
    publishedEvents = publishedEvents :+ event
  }
}
