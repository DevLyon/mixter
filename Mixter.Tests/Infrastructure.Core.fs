namespace Mixter.Tests.Infrastructure.Core

open Swensen.Unquote
open Xunit
open Mixter.Infrastructure.Core

module ``MemoryTimelineMessageStore should`` =
    open Mixter.Domain.Identity.UserIdentity
    open Mixter.Domain.Core.Message
    open Mixter.Domain.Core.Timeline

    [<Fact>]
    let ``return messages of user when GetMessagesOfUser`` () =
        let repository = MemoryTimelineMessageStore()
        let timelineMessage = { Owner = { Email = "A" }; Author = { Email = "A" }; Content = "Hello"; MessageId = MessageId.Generate() }

        repository.Save timelineMessage

        test <@ repository.GetMessagesOfUser timelineMessage.Owner |> Seq.toList
                    = [timelineMessage] @>

    [<Fact>]
    let ``save only one message when save two same message`` () =
        let repository = MemoryTimelineMessageStore()
        let timelineMessage = { Owner = { Email = "A" }; Author = { Email = "A" }; Content = "Hello"; MessageId = MessageId.Generate() }

        repository.Save timelineMessage
        repository.Save timelineMessage

        test <@ repository.GetMessagesOfUser timelineMessage.Owner |> Seq.toList
                  = [timelineMessage] @>

    [<Fact>]
    let ``Remove message of all users when remove this message`` () =
        let repository = MemoryTimelineMessageStore()
        let messageId = MessageId.Generate()
        let user1 = { Email = "A" }
        let user2 = { Email = "B" }
        repository.Save { Owner = user1; Author = user1; Content = "Hello"; MessageId = messageId }
        repository.Save { Owner = user2; Author = user2; Content = "Hello"; MessageId = messageId }

        repository.Delete messageId

        test <@ repository.GetMessagesOfUser user1 |> Seq.isEmpty @>
        test <@ repository.GetMessagesOfUser user2 |> Seq.isEmpty @>

module ``MemoryFollowersRepository should`` =
    open Mixter.Domain.Identity.UserIdentity
    open Mixter.Domain.Core.SubscriptionProjection
    
    [<Fact>]
    let ``Return FollowerId When GetFollowers after save`` () =
        let repository = MemoryFollowersRepository()
        let followee = { Email = "followee@mixit.fr" }
        let follower = { Email = "follower@mixit.fr" }
        repository.Save { Followee = followee; Follower = follower }

        test <@ repository.GetFollowers followee |> Seq.toList = [follower] @>

    [<Fact>]
    let ``return only follower ids of followee When GetFollowers`` () =
        let repository = MemoryFollowersRepository()
        let followee1 = { Email = "followee1@mixit.fr" }
        let followee2 = { Email = "followee2@mixit.fr" }
        let follower1 = { Email = "follower1@mixit.fr" }
        let follower2 = { Email = "follower2@mixit.fr" }
        repository.Save { Followee = followee1; Follower = follower1 }
        repository.Save { Followee = followee2; Follower = follower2 }

        test <@ repository.GetFollowers followee1 |> Seq.toList = [follower1] @>
            
    [<Fact>]
    let ``Return empty When GetFollowers after remove follower`` () =
        let repository = MemoryFollowersRepository()
        let followee = { Email = "followee@mixit.fr" }
        let follower = { Email = "follower@mixit.fr" }
        repository.Save { Followee = followee; Follower = follower }
        repository.Delete { Followee = followee; Follower = follower }

        test <@ repository.GetFollowers followee |> Seq.isEmpty @>

    [<Fact>]
    let ``Return only one follower When save several times same follower`` () =
        let repository = MemoryFollowersRepository()
        let followee = { Email = "followee@mixit.fr" }
        let follower = { Email = "follower@mixit.fr" }
        repository.Save { Followee = followee; Follower = follower }
        repository.Save { Followee = followee; Follower = follower }

        test <@ repository.GetFollowers followee |> Seq.toList = [follower] @>
