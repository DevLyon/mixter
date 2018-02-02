module Mixter.Domain.Core.Subscription

open Mixter.Domain.Identity.UserIdentity
open Mixter.Domain.Core.Message

type SubscriptionId = { Follower: UserId; Followee: UserId }
