package mixter.domain

import org.scalatest.{Suite, SuiteMixin}

class SpyEventPublisher extends EventPublisher {
  var publishedEvents:Seq[Event]=Seq.empty
  override def publish[T<:Event](event:T): Unit = {
    publishedEvents = publishedEvents :+ event
  }
}
trait SpyEventPublisherFixture extends SuiteMixin { this: Suite =>
  def withSpyEventPublisher(test: SpyEventPublisher=>Any): Any = {
    val spyEventPublisher= new SpyEventPublisher()
    test(spyEventPublisher)
  }
}