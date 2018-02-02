namespace Mixter.Tests.Domain.Core.Timeline

open Xunit
open Swensen.Unquote
open Mixter.Domain.Identity.UserIdentity
open Mixter.Domain.Core.Message
open Mixter.Domain.Core.Timeline
open Mixter.Infrastructure.Core

module ``Timeline should`` =
    [<Fact>] 
    let ``When handle MessageQuacked Then save TimelineMessage projection for author`` () =
        let repository = MemoryTimelineMessageStore()
        let messageQuacked = { MessageId = MessageId.Generate(); AuthorId = { Email = "A" }; Content = "Hello" }

        MessageQuacked messageQuacked |> handle repository.Save

        test <@ repository.GetMessagesOfUser messageQuacked.AuthorId |> Seq.toList
                 = [{ 
                        Owner = messageQuacked.AuthorId
                        Author = messageQuacked.AuthorId
                        Content = messageQuacked.Content
                        MessageId = messageQuacked.MessageId 
                    }] @>
