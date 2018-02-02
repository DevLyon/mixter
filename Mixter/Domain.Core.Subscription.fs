module Mixter.Domain.Core.Subscription

open Mixter.Domain.Documentation
open Mixter.Domain.Identity.UserIdentity
open Mixter.Domain.Core.Message

type SubscriptionId = { Follower: UserId; Followee: UserId }

[<Event>]
type Event = 
    | UserFollowed of UserFollowed
    | UserUnfollowed of UserUnfollowed
    | FolloweeMessageQuacked of FolloweeMessageQuacked
and UserFollowed = { SubscriptionId: SubscriptionId }
and UserUnfollowed = { SubscriptionId: SubscriptionId }
and FolloweeMessageQuacked = { SubscriptionId: SubscriptionId; Message: MessageId }

[<Projection>]
type DecisionProjection =
    | NoSubscription
    | Active of SubscriptionId
    | Disable of SubscriptionId

let applyOne decisionProjection event =
    match event with
    | UserFollowed e -> Active e.SubscriptionId
    | _ -> decisionProjection

let apply events =
    Seq.fold applyOne NoSubscription events

[<Command>]
let follow follower followee =
    [ UserFollowed { SubscriptionId = { Follower = follower; Followee = followee } } ]

[<Command>]
let unfollow history =
    match history |> apply with
    | Active subscriptionId -> [ UserUnfollowed { SubscriptionId = subscriptionId } ]
    | _ -> []

[<Command>]
let notifyFollower message history =
    match history |> apply with
    | Active subscriptionId -> [ FolloweeMessageQuacked { SubscriptionId = subscriptionId; Message = message } ]
    | _ -> []
