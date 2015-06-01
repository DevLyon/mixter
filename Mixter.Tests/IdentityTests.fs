namespace Mixter.Domain.Identity.Tests

open NUnit.Framework
open FsUnit
open System
open Mixter.Domain.Identity

[<TestFixture>]
type ``User aggregate`` ()=

    [<Test>] 
    member x.``When a user register, then user registered event is returned`` () =
        register (UserId "clem@mix-it.fr" ) 
            |> should equal [ UserRegistered { UserId = UserId "clem@mix-it.fr" } ]

    [<Test>]
    member x.``When a user log in, then user connected event is returned`` () =
        let sessionId = SessionId.generate
        let getCurrentTime = new DateTime()
        apply DecisionProjection.initial (UserRegistered { UserId = UserId "clem@mix-it.fr" })
            |> logIn sessionId getCurrentTime
            |> should equal [ UserConnected { SessionId = sessionId; UserId = UserId "clem@mix-it.fr"; ConnectedAt = getCurrentTime } ]