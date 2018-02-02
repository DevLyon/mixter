namespace Mixter.Tests.Domain.Core.Timeline

open Xunit
open Swensen.Unquote
open Mixter.Domain.Identity.UserIdentity
open Mixter.Domain.Core.Message
open Mixter.Domain.Core.Timeline
open Mixter.Infrastructure.Core

module ``Timeline`` =
    [<Fact>] 
    let ``When handle MessageQuacked Then save TimelineMessage projection for author`` () =
        let repository = MemoryTimelineMessageStore()
        let messageQuacked = { MessageId = MessageId.Generate(); AuthorId = { Email = "A" }; Content = "Hello" }

        MessageQuacked messageQuacked |> handle repository.Save repository.Delete

        test <@ repository.GetMessagesOfUser messageQuacked.AuthorId |> Seq.toList
                 = [{ 
                        Owner = messageQuacked.AuthorId
                        Author = messageQuacked.AuthorId
                        Content = messageQuacked.Content
                        MessageId = messageQuacked.MessageId 
                    }] @>

    [<Fact>] 
    let ``When handle MessageDeleted Then remove this message in timeline`` () =
        let repository = MemoryTimelineMessageStore()
        let messageId = MessageId.Generate()
        let author = { Email = "A" }

        MessageQuacked { MessageId = messageId; AuthorId = author; Content = "Hello" } 
        |> handle repository.Save repository.Delete

        MessageDeleted { MessageId = messageId; Deleter = author }
        |> handle repository.Save repository.Delete 

        test <@ repository.GetMessagesOfUser author |> Seq.isEmpty @>
