package infra

import domain.Event
import org.scalatest.{Matchers, WordSpec}

class InMemoryEventStoreSpec extends WordSpec with Matchers {

  val AGGREGATE_ID1 = AnAggregateId()
  "An empty event store" should {
    "return an empty list when getting events of an aggregate" in {
      // Given
      val eventStore = new InMemoryEventStore()
      val aggregateId1 = AGGREGATE_ID1

      // When
      val eventsOfAggregate:Seq[Event] = eventStore.eventsOfAggregate(aggregateId1)

      // Then
      eventsOfAggregate shouldBe empty
    }
    "return the event for an aggregate when an event was just saved for this aggregate" in {
      // Given
      val eventStore = new InMemoryEventStore()
      val aggregateId1 = AGGREGATE_ID1
      val expected = EventA(aggregateId1)

      // When
      eventStore.store(expected)
      val eventsOfAggregate = eventStore.eventsOfAggregate(aggregateId1)

      // Then
      eventsOfAggregate should contain theSameElementsAs Seq(expected)
    }
  }
}
