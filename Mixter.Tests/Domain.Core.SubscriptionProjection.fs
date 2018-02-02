namespace Mixter.Tests.Domain.Core.SubscriptionProjection

open Xunit
open Swensen.Unquote

module ``SubscriptionProjection should`` =
    open Mixter.Domain.Core.Subscription
    open Mixter.Domain.Identity.UserIdentity
    open Mixter.Domain.Core.SubscriptionProjection 
    open Mixter.Infrastructure.Core

    [<Fact>] 
    let ``Save follower When UserFollowed`` () =
        let followerRepository = MemoryFollowersRepository()
        let handle = handle followerRepository.Save followerRepository.Delete
        let follower = { Email = "follower@mix-it.fr" } 
        let followee = { Email = "followee@mix-it.fr" }

        UserFollowed { SubscriptionId = { Follower = follower; Followee = followee }}
        |> handle

        test <@ followerRepository.GetFollowers followee |> Seq.toList = [ follower ] @>
