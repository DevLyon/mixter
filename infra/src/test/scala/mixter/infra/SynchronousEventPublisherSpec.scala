package mixter.infra

import org.scalatest.matchers.{MatchResult, Matcher}
import org.scalatest.{Matchers, WordSpec}

class SynchronousEventPublisherSpec extends WordSpec with Matchers {

 "A SyncrhonousEventPublisher with a registered handler" should {
   "call the handler when an event is published" in {
     // Given
     val handler=new SpyEventHandler[EventA]()
     val publisher = new SynchronousEventPublisher()
     val event = EventA(AnAggregateId("id"))
     publisher.register[EventA](handler)

     // When
     publisher.publish(event)

     // Then
     handler should haveBeenCalled
   }
 }
 "A SyncrhonousEventPublisher with 2 registered handler" should {
    "call both handlers when an event is published" in {
      // Given
      val handler1=new SpyEventHandler[EventA]()
      val handler2=new SpyEventHandler[EventA]()
      val publisher = new SynchronousEventPublisher()
      val event = EventA(AnAggregateId("id"))
      publisher.register[EventA](handler1)
      publisher.register[EventA](handler2)

      // When
      publisher.publish(event)

      // Then
      handler1 should haveBeenCalled
      handler2 should haveBeenCalled
    }
  }
  "A SyncrhonousEventPublisher with 2 registered handler each for a different event" should {
    "only call the handler for the event which is published" in {
      // Given
      val handler1=new SpyEventHandler[EventA]()
      val handler2=new SpyEventHandler[EventB]()
      val publisher = new SynchronousEventPublisher()
      val event = EventA(AnAggregateId("id"))
      publisher.register[EventA](handler1)
      publisher.register[EventB](handler2)

      // When
      publisher.publish(event)

      // Then
      handler1 should haveBeenCalled
      handler2 should not(haveBeenCalled)
    }
  }

  val haveBeenCalled= new Matcher[SpyEventHandler[_]]{
    override def apply(left: SpyEventHandler[_]): MatchResult = {
      MatchResult(
        left.wasCalled,
        s"""the event handler was not called but it should have""",
        s"""the event handler was called but it shouldn't have"""
      )
    }
  }
}
