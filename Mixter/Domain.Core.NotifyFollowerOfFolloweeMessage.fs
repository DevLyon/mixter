module Mixter.Domain.Core.NotifyFollowerOfFolloweeMessage

open Mixter.Domain.Documentation
open Mixter.Domain.Identity.UserIdentity
open Mixter.Domain.Core.Subscription
open Mixter.Domain.Core.Message

type GetSubscriptionHistory = SubscriptionId -> Subscription.Event list
type GetFollowers = UserId -> UserId seq

let notifyAllFollowers (getSubscriptionHistory: GetSubscriptionHistory) (getFollowers: GetFollowers) (followee: UserId) messageId =
    getFollowers followee 
    |> Seq.collect (fun follower -> 
        { Follower = follower; Followee = followee }
        |> getSubscriptionHistory 
        |> Subscription.notifyFollower messageId)

[<Handler>]
let handle 
        (getSubscriptionHistory: GetSubscriptionHistory) 
        (getFollowers: GetFollowers) 
        (evt: Message.Event) 
        : Subscription.Event seq =
    let notifyAllFollowers = notifyAllFollowers getSubscriptionHistory getFollowers

    match evt with
    | MessageQuacked e -> notifyAllFollowers e.AuthorId e.MessageId
    | _ -> Seq.empty

