namespace Mixter.Infrastructure.Identity.Read.Tests

open NUnit.Framework
open FsUnit
open Mixter.Domain.Identity
open Mixter.Domain.Identity.Read
open Mixter.Infrastructure.Identity.Read
open System.Collections.Generic

[<TestFixture>]
type ``Given a repository of session projection`` ()=

    [<Test>]
    member x.``Given repository contains two session projection, when get a session by its id, then it returns the corresponding session projection`` () =
        let sessionId = SessionId.generate
        let anotherSessionId = SessionId.generate
        let sessions = new MemorySessionsStore()
        Add { SessionId = sessionId; UserId = UserId "clem@mix-it.fr" } |> sessions.ApplyChange
        Add { SessionId = anotherSessionId; UserId = UserId "clem@mix-it.fr" } |> sessions.ApplyChange

        sessions.GetSession sessionId
            |> should equal (Some { UserId = UserId "clem@mix-it.fr"; SessionId = sessionId })
