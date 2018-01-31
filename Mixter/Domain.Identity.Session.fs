module Mixter.Domain.Identity.Session

open Mixter.Domain.Identity.UserIdentity
open Mixter.Domain.Documentation

type SessionId = SessionId of string
    with static member Generate () = SessionId (System.Guid.NewGuid().ToString())

[<Event>]
type Event = 
    | UserConnected of UserConnectedEvent
    | UserDisconnected of UserDisconnectedEvent
and UserConnectedEvent = { SessionId: SessionId; UserId: UserId; ConnectedAt: System.DateTime}
and UserDisconnectedEvent = { SessionId: SessionId; UserId: UserId }

[<Projection>]
type DecisionProjection = 
    | NotConnectedUser
    | ConnectedUser of ConnectedUser
    | DisconnectedUser of DisconnectedUser
and DisconnectedUser = { UserId: UserId }
and ConnectedUser = { UserId: UserId; SessionId: SessionId }
    
let private applyOne decisionProjection event =
    match (decisionProjection, event) with
    | (NotConnectedUser _, UserConnected e) -> ConnectedUser { UserId = e.UserId; SessionId = e.SessionId }
    | (ConnectedUser _, UserDisconnected e) -> DisconnectedUser { UserId = e.UserId }
    | _ -> failwith "Invalid transition"

let private apply history =
    Seq.fold applyOne NotConnectedUser history

[<Command>]
let logIn userId sessionId currentTime =
    [ UserConnected { SessionId = sessionId; UserId = userId; ConnectedAt = currentTime } ]

[<Command>]
let logOut history =
    match history |> apply with
    | ConnectedUser p -> 
        [ UserDisconnected { SessionId = p.SessionId; UserId = p.UserId } ]
    | _ -> []
