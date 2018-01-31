module Mixter.Tests.Domain.Identity.Session

open Xunit
open Swensen.Unquote
open System
open Mixter.Domain.Identity.UserIdentity;
open Mixter.Domain.Identity.Session;

module ``Session should`` =

    [<Fact>]
    let ``Return UserConnected When logs in`` () =
        let sessionId = SessionId.Generate()
        let currentTime = DateTime.Now
        let user = { Email = "clem@mix-it.fr" }

        test <@ logIn user sessionId currentTime
                    = [ UserConnected { SessionId = sessionId; UserId = user; ConnectedAt = currentTime } ] @>

    [<Fact>]
    let ``Return UserDisconnected When disconnect`` () =
        let sessionId = SessionId.Generate()
        let userId = { Email = "clem@mix-it.fr" }
        let history = [ UserConnected { SessionId = sessionId; UserId = userId; ConnectedAt = DateTime.Now }]
        
        test <@ history |> logOut
                    = [UserDisconnected { SessionId = sessionId; UserId = userId } ] @>
    
    [<Fact>]
    let ``Return nothing When disconnect If session have been disconnected`` () =
        let sessionId = SessionId.Generate()
        let userId = { Email = "clem@mix-it.fr" }
        let history = [  
            UserConnected { SessionId = sessionId; UserId = userId; ConnectedAt = DateTime.Now };
            UserDisconnected { SessionId = sessionId; UserId = userId } 
        ]   
        
        test <@ history |> logOut |> Seq.isEmpty @>
        