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
        apply DecisionProjection.initial [ MessagePublished { MessageId = messageId; UserId = UserId "clem@mix-it.fr"; Content = "hello world" } ]
            |> republish
            |> should equal [ MessageRepublished { MessageId = messageId } ]
