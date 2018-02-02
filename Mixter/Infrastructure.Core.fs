﻿module Mixter.Infrastructure.Core

open Mixter.Domain.Documentation
open Mixter.Domain.Core
open Mixter.Domain.Core.Timeline

open System.Collections.Generic

[<Repository>]
type MemoryTimelineMessageStore() =
    let store = HashSet<TimelineMessage>()

    member _.Save timelineMessage =
        store.Add timelineMessage |> ignore

    [<Query>]
    member _.GetMessagesOfUser userId =
        store |> Seq.filter (fun p -> p.Owner = userId)
