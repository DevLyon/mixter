namespace Mixter.Tests.Domain.Core.NotifyFollowerOfFolloweeMessage

open Xunit
open Swensen.Unquote
open Mixter.Infrastructure.EventsStore
open Mixter.Domain.Core.Message

module ``NotifyFollowerOfFolloweeMessage should`` =
    open Mixter.Domain.Core.Subscription
    open Mixter.Domain.Identity.UserIdentity
    open Mixter.Domain.Core.NotifyFollowerOfFolloweeMessage
    open Mixter.Infrastructure.Core

    [<Fact>] 
    let ``Raise FolloweeMessageQuacked When MessageQuacked by followee`` () =
        let followerRepository = MemoryFollowersRepository()
        let eventsStore = MemoryEventsStore()
        let follower = { Email = "follower@mix-it.fr" } 
        let followee = { Email = "followee@mix-it.fr" }
        followerRepository.Save { Follower = follower; Followee = followee }
        let subscriptionId = {Follower = follower; Followee = followee}
        eventsStore.Store subscriptionId [UserFollowed { SubscriptionId = subscriptionId}]
        let handle = handle (eventsStore.Get) (followerRepository.GetFollowers)
        let messageId = MessageId.Generate()

        test <@ MessageQuacked { MessageId = messageId; AuthorId = followee; Content = "hello" }
                |> handle |> Seq.toList = [ FolloweeMessageQuacked { SubscriptionId = subscriptionId; Message = messageId } ] @>

    [<Fact>] 
    let ``Raise FolloweeMessageQuacked When MessageRequacked by followee`` () =
        let followerRepository = MemoryFollowersRepository()
        let eventsStore = MemoryEventsStore()
        let follower = { Email = "follower@mix-it.fr" } 
        let followee = { Email = "followee@mix-it.fr" }
        followerRepository.Save { Follower = follower; Followee = followee }
        let subscriptionId = {Follower = follower; Followee = followee}
        eventsStore.Store subscriptionId [UserFollowed { SubscriptionId = subscriptionId}]
        let handle = handle (eventsStore.Get) (followerRepository.GetFollowers)
        let messageId = MessageId.Generate()

        test <@ MessageRequacked { MessageId = messageId; Requacker = followee }
                |> handle |> Seq.toList = [ FolloweeMessageQuacked { SubscriptionId = subscriptionId; Message = messageId } ] @>
        