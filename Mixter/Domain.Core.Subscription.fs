module Mixter.Domain.Core.Subscription

open Mixter.Domain.Documentation
open Mixter.Domain.Identity.UserIdentity
open Mixter.Domain.Core.Message

type SubscriptionId = { Follower: UserId; Followee: UserId }

[<Event>]
type Event = 
    | UserFollowed of UserFollowed
and UserFollowed = { SubscriptionId: SubscriptionId }

[<Command>]
let follow follower followee =
    [ UserFollowed { SubscriptionId = { Follower = follower; Followee = followee } } ]
