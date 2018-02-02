module Mixter.Domain.Core.NotifyFollowerOfFolloweeMessage

open Mixter.Domain.Documentation
open Mixter.Domain.Identity.UserIdentity
open Mixter.Domain.Core.Subscription
open Mixter.Domain.Core.Message

type GetSubscriptionHistory = SubscriptionId -> Subscription.Event list
type GetFollowers = UserId -> UserId seq

[<Handler>]
let handle 
        (getSubscriptionHistory: GetSubscriptionHistory) 
        (getFollowers: GetFollowers) 
        (evt: Message.Event) 
        : Subscription.Event seq =
    Seq.empty
