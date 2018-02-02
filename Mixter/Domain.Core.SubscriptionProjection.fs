module Mixter.Domain.Core.SubscriptionProjection

open Mixter.Domain.Documentation
open Mixter.Domain.Identity.UserIdentity
open Mixter.Domain.Core.Subscription

[<Projection>]
type SubscriptionProjection = { Followee: UserId; Follower: UserId }
