namespace Mixter.Tests.Domain.Core.Subscription

open Xunit
open Swensen.Unquote
open Mixter.Domain.Identity.UserIdentity
open Mixter.Domain.Core.Message
open Mixter.Domain.Core.Subscription

module ``Subscription should`` =
    [<Fact>] 
    let ``When follow Then UserFollowed is returned`` () =
        let follower = { Email = "follower@mix-it.fr" } 
        let followee = { Email = "followee@mix-it.fr" }

        test <@ follow follower followee
                    = [UserFollowed { SubscriptionId = { Follower = follower; Followee = followee } }] @>
