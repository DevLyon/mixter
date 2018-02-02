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
            