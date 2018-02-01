namespace Mixter.Tests.Infrastructure.EventsStore

open Xunit
open Swensen.Unquote
open Mixter.Infrastructure.EventsStore

type Event =
    | EventA of string
    | EventB of string

module ``MemoryEventsStore should`` =
    [<Fact>] 
    let ``Return aggregate events When store events of aggregate and get events for this aggregate`` () =
        let eventsStore = MemoryEventsStore()

        let aggregateId = "AggregateA"
        let events = [ EventA "A"; EventB "B"; EventA "C"]
        
        events |> eventsStore.Store aggregateId

        test <@ eventsStore.Get<Event> aggregateId = events @>

    [<Fact>] 
    let ``Return old and new events When store new events for an aggregate and get events`` () =
        let eventsStore = MemoryEventsStore()
        let aggregateId = "AggregateA"
        let oldEvents = [ EventA "A"; EventB "B"; EventA "C"]
        oldEvents |> eventsStore.Store aggregateId

        let newEvents = [ EventA "D"; EventB "E"]
        newEvents |> eventsStore.Store aggregateId

        test <@ eventsStore.Get<Event> aggregateId = (oldEvents @ newEvents) @>

    [<Fact>] 
    let ``return only events of aggregate When store events for another aggregate and get events for this aggregate`` () =
        let eventsStore = MemoryEventsStore()
        let aggregateIdA = "AggregateA"
        let aggregateIdB = "AggregateB"
        let eventsOfAggregateA = [ EventA "A"; EventB "B"; EventA "C"]
        eventsOfAggregateA |> eventsStore.Store aggregateIdA

        let eventsOfAggregateB = [ EventA "D"; EventB "E"]
        eventsOfAggregateB |> eventsStore.Store aggregateIdB

        test <@ eventsStore.Get<Event> aggregateIdB = eventsOfAggregateB @>
        
    [<Fact>] 
    let ``return no event When get events for unknown aggregate `` () =
        let eventsStore = MemoryEventsStore()

        test <@ eventsStore.Get<Event> "Unknown" |> Seq.isEmpty @>
