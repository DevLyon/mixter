module Mixter.Infrastructure.Core

open Mixter.Domain.Documentation
open Mixter.Domain.Core
open Mixter.Domain.Core.Timeline

open System.Collections.Generic

[<Repository>]
type MemoryTimelineMessageStore() =
    let store = new HashSet<TimelineMessage>()

    member __.Save timelineMessage =
        store.Add timelineMessage |> ignore

    [<Query>]
    member __.GetMessagesOfUser userId =
        store |> Seq.filter (fun p -> p.Owner = userId)
