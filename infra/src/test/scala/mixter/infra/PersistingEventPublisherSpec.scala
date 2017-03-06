package mixter.infra

import org.scalatest.matchers.should.Matchers
import org.scalatest.wordspec.AnyWordSpec

class PersistingEventPublisherSpec extends AnyWordSpec with Matchers {
  "A PersistingEventPublisher" should {
    "store an event when it is published" in {
      //Given
      val store = new InMemoryEventStore()
      val publisher = new SpyEventPublisher()
      val event = EventA(AnAggregateId("id"))
      val persistedPublisher = new PersistingEventPublisher(store, publisher)

      // When
      persistedPublisher.publish(event)

      // Then
      store.eventsOfAggregate(event.id) should contain theSameElementsAs Seq(event)
    }
  }
}
