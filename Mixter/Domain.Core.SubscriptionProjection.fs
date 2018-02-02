module Mixter.Domain.Core.SubscriptionProjection

open Mixter.Domain.Documentation
open Mixter.Domain.Identity.UserIdentity
open Mixter.Domain.Core.Subscription

[<Projection>]
type SubscriptionProjection = { Followee: UserId; Follower: UserId }

let private project (subscriptionId: SubscriptionId) =
    { Followee = subscriptionId.Followee; Follower = subscriptionId.Follower }

[<Handler>]
let handle save remove evt =
    match evt with
    | UserFollowed e -> project e.SubscriptionId |> save
    | UserUnfollowed e -> project e.SubscriptionId |> remove
    | _ -> ()