package infra

import domain.Event
import org.scalatest.{Matchers, WordSpec}

class InMemoryEventStoreSpec extends WordSpec with Matchers {

  val AGGREGATE_ID1 = AnAggregateId("1")
  val AGGREGATE_ID2 = AnAggregateId("2")
  val AGGREGATE_ID3 = AnAggregateId("3")

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
  "A store containing events of 3 aggregates" should {
    "should return only the events of one aggregate when getting events for this aggregate" in {
      //Given
      val eventStore = new InMemoryEventStore()
      val eventA = EventA(AGGREGATE_ID1)
      eventStore.store(eventA)
      eventStore.store(EventA(AGGREGATE_ID2))
      eventStore.store(EventA(AGGREGATE_ID3))

      //When
      val eventsOfAggregate = eventStore.eventsOfAggregate(AGGREGATE_ID1)

      //Then
      eventsOfAggregate should contain theSameElementsAs Seq(eventA)
    }
  }
}
