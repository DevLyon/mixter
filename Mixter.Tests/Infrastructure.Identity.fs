namespace Mixter.Tests.Infrastructure.Identity

open Xunit
open Swensen.Unquote
open Mixter.Domain.Identity.UserIdentity
open Mixter.Domain.Identity.Session
open Mixter.Domain.Identity.SessionDescription
open Mixter.Infrastructure.Identity

module ``MemorySessionsStore should`` =

    [<Fact>]
    let ``Returns the corresponding session projection When get a session by its id`` () =
        let sessionId = SessionId.Generate ()
        let anotherSessionId = SessionId.Generate ()
        let sessions = MemorySessionsStore()
        sessions.Add sessionId { UserId = { Email = "user1@mix-it.fr" } }
        sessions.Add anotherSessionId { UserId = { Email = "user2@mix-it.fr" } }

        test <@ sessions.GetSession sessionId
                    = (Some { UserId = { Email = "user1@mix-it.fr" } }) @>

    [<Fact>]
    let ``Return sessionId When GetUserSession with userId`` () =
        let sessionId = SessionId.Generate ()
        let sessions = MemorySessionsStore()
        let userId = { Email = "user1@mix-it.fr" }
        sessions.Add sessionId { UserId = userId }

        test <@ sessions.GetUserSession userId = Some sessionId @>

    [<Fact>]
    let ``Return None When GetUserSession with unknown userId`` () =
        let sessions = MemorySessionsStore()
        let userId = { Email = "user1@mix-it.fr" } 

        test <@ sessions.GetUserSession userId = None @>
            
    [<Fact>]
    let ``Return None When GetUserSession if session is removed`` () =
        let sessionId = SessionId.Generate ()
        let sessions = MemorySessionsStore()
        let userId = { Email = "user1@mix-it.fr" } 
        sessions.Add sessionId { UserId = userId }

        sessions.Remove sessionId

        test <@ sessions.GetUserSession userId = None @>
            