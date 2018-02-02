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

    member __.Delete messageId =
        store.RemoveWhere(fun p -> p.MessageId = messageId) |> ignore

[<Repository>]
type MemoryFollowersRepository() =
    let store = new HashSet<SubscriptionProjection.SubscriptionProjection>()

    member __.Save projection =
        store.Add projection |> ignore

    [<Query>]
    member __.GetFollowers followee =
        store 
        |> Seq.filter (fun p -> p.Followee = followee)
        |> Seq.map (fun p -> p.Follower)

    member __.Delete projection =
        store.Remove projection |> ignore
