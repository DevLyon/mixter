namespace Mixter.Domain.Identity.Tests

open NUnit.Framework
open FsUnit
open System
open Mixter.Domain.Identity

[<TestFixture>]
type ``Given a User`` ()=

    [<Test>] 
    member x.``When he registers, then user registered event is returned`` () =
        register (UserId "clem@mix-it.fr" ) 
            |> should equal [ UserRegistered { UserId = UserId "clem@mix-it.fr" } ]

    [<Test>]
    member x.``When he logs in, then user connected event is returned`` () =
        let sessionId = SessionId.generate
        let getCurrentTime = fun () -> new DateTime()
        apply DecisionProjection.initial [ UserRegistered { UserId = UserId "clem@mix-it.fr" } ]
            |> logIn sessionId getCurrentTime
            |> should equal [ UserConnected { SessionId = sessionId; UserId = UserId "clem@mix-it.fr"; ConnectedAt = getCurrentTime () } ]

[<TestFixture>]
type ``Given a started session`` ()=

    [<Test>]
    member x.``When disconnect, then user disconnected event is returned`` () =
        let sessionId = SessionId.generate
        let userId = UserId "clem@mix-it.fr"
        let getCurrentTime = fun () -> new DateTime()
        [ UserRegistered { UserId = userId }; UserConnected { SessionId = sessionId; UserId = userId; ConnectedAt = getCurrentTime () }]
            |> apply DecisionProjection.initial
            |> logOut
            |> should equal [ UserDisconnected { SessionId = sessionId; UserId = userId } ]
    
    [<Test>]
    member x.``Given session have been disconnected, when disconnect, then nothing happen`` () =
        let sessionId = SessionId.generate
        let userId = UserId "clem@mix-it.fr"
        let getCurrentTime = fun () -> new DateTime()
        [ UserRegistered { UserId = userId }; 
        UserConnected { SessionId = sessionId; UserId = userId; ConnectedAt = getCurrentTime () };
        UserDisconnected { SessionId = sessionId; UserId = userId } ]
            |> apply DecisionProjection.initial
            |> logOut
            |> should equal []

open Read

[<TestFixture>]
type ``Given a handler of session events`` ()=
    let sessionId = SessionId.generate
    let userId = UserId "clem@mix-it.fr"
    let sessions = fun x -> Some { SessionId = sessionId; UserId = userId }
    
    [<Test>]
    member x.``When project user connected, then it returns a Session projection`` () =
        let userConnected = UserConnected { SessionId = sessionId; UserId = userId; ConnectedAt = DateTime.Now}
        let expectedAddSession = Add { SessionId = sessionId; UserId = userId }
        project sessions userConnected
            |> should equal expectedAddSession
            
    [<Test>]
    member x.``When project user disconnected, then it returns a Remove change of Session projection`` () =
        let userConnected = UserDisconnected { SessionId = sessionId; UserId = userId }
        let expectedRemoveSession = Remove { SessionId = sessionId; UserId = userId }
        project sessions userConnected
            |> should equal expectedRemoveSession