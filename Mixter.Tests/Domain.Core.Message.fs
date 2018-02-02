namespace Mixter.Tests.Domain.Core.Message

open Xunit
open Swensen.Unquote
open Mixter.Domain.Identity.UserIdentity
open Mixter.Domain.Core.Message

module ``MessageId should`` =
    [<Fact>] 
    let ``Return unique id when generate`` () =
        let messageId1 = MessageId.Generate()
        let messageId2 = MessageId.Generate()

        test <@ messageId1 <> messageId2 @>

module ``Message should`` =

    [<Fact>] 
    let ``return MessageQuacked When quack`` () =
        let messageId = MessageId.Generate()
        let userId = { Email = "clem@mix-it.fr"}

        test <@ quack messageId userId "hello world" 
                  = [ MessageQuacked { MessageId = messageId; AuthorId = userId; Content = "hello world" } ] @>

    [<Fact>] 
    let ``return MessageRequacked When requack`` () =
        let messageId = MessageId.Generate()
        let requaker = { Email = "someone@mix-it.fr"}

        let result =
            [ MessageQuacked { MessageId = messageId; AuthorId = { Email = "clem@mix-it.fr" }; Content = "hello world" } ]
            |> requack requaker

        test <@ result = [ MessageRequacked { MessageId = messageId; Requacker = requaker } ] @>
            
    [<Fact>] 
    let ``return nothing When author requack`` () =
        let messageId = MessageId.Generate()
        let authorId = { Email = "clem@mix-it.fr" }

        let result = 
            [ MessageQuacked { MessageId = messageId; AuthorId = authorId; Content = "hello world" } ]
            |> requack authorId

        test <@ result |> Seq.isEmpty @>
            
    [<Fact>] 
    let ``return nothing When requack two times same message`` () =
        let messageId = MessageId.Generate()
        let authorId = { Email = "author@mix-it.fr" }
        let requackerId = { Email = "requacker@mix-it.fr" }

        let result =
            [ 
                MessageQuacked { MessageId = messageId; AuthorId = authorId; Content = "hello world" }
                MessageRequacked { MessageId = messageId; Requacker = requackerId }
            ]   
            |> requack requackerId
            
        test <@ result |> Seq.isEmpty @>
            
    [<Fact>] 
    let ``return MessageDeleted When delete`` () =
        let messageId = MessageId.Generate()
        let authorId = { Email = "author@mix-it.fr" }

        let result =
            [ 
                MessageQuacked { MessageId = messageId; AuthorId = authorId; Content = "hello world" }
            ]   
            |> delete authorId
        
        test <@ result = [ MessageDeleted { MessageId = messageId; Deleter = authorId } ] @>
