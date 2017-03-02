package infra

import domain.Event
import org.scalatest.{Matchers, WordSpec}

class InMemoryEventStoreSpec extends WordSpec with Matchers {
  //GivenAnEmptyEventStoreWhenGettingEventsOfAggregateThenItReturnsAnEmptyList() {
  val AGGREGATE_ID1 = AnAggregateId()
  "An empty event store" should {
    val eventStore = new InMemoryEventStore()
    "return an empty list when getting events of an aggregate" in {
      // Given
      val aggregateId1 = AGGREGATE_ID1

      // When
      val eventsOfAggregate:Seq[Event] = eventStore.eventsOfAggregate(aggregateId1)

      // Then
      eventsOfAggregate shouldBe empty
    }
  }
}
