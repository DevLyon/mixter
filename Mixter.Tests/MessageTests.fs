namespace Mixter.Domain.Message.Tests

open NUnit.Framework
open FsUnit
open System
open Mixter.Domain.Identity
open Mixter.Domain.Message

[<TestFixture>]
type ``Given a Message`` ()=
    [<Test>] 
    member x.``When publish, then user published event is returned`` () =
        let messageId = MessageId.generate
        publish messageId (UserId "clem@mix-it.fr") "hello world" 
            |> should equal [ MessagePublished { MessageId = messageId; UserId = UserId "clem@mix-it.fr"; Content = "hello world" } ]

    [<Test>] 
    member x.``When republish, then user republished event is returned`` () =
        let messageId = MessageId.generate
        [ MessagePublished { MessageId = messageId; UserId = UserId "clem@mix-it.fr"; Content = "hello world" } ]
            |> apply
            |> republish (UserId "someone@mix-it.fr")
            |> should equal [ MessageRepublished { MessageId = messageId } ]
            
    [<Test>] 
    member x.``When author republish, then nothing is returned`` () =
        let messageId = MessageId.generate
        let authorId = UserId "clem@mix-it.fr"
        [ MessagePublished { MessageId = messageId; UserId = authorId; Content = "hello world" } ]
            |> apply 
            |> republish authorId
            |> should equal []
