module Mixter.Tests.Domain.Identity.SessionDescription

open Xunit
open Swensen.Unquote
open System
open Mixter.Domain.Identity.UserIdentity;
open Mixter.Domain.Identity.Session;
open Mixter.Domain.Identity.SessionDescription;
open Mixter.Infrastructure.Identity;

module ``Session events handler should`` =
    let sessionId = SessionId.Generate()
    let userId = { Email = "clem@mix-it.fr"}
    
    [<Fact>]
    let ``Add a SessionDescription When handle UserConnected`` () =
        let userConnected = UserConnected { 
            SessionId = sessionId
            UserId = userId
            ConnectedAt = DateTime.Now
        }
        let sessionsStore = MemorySessionsStore()
        let handle = handle sessionsStore.Add sessionsStore.Remove

        handle userConnected

        test <@ sessionsStore.GetSession sessionId = Some { UserId = userId } @>
            
    [<Fact>]
    let ``Remove Session When handle user disconnected`` () =
        let userConnected = UserDisconnected { 
            SessionId = sessionId
            UserId = userId 
        }
        let sessionsStore = MemorySessionsStore()
        let handle = handle sessionsStore.Add sessionsStore.Remove

        handle userConnected

        test <@ sessionsStore.GetSession sessionId = None @>
