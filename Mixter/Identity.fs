module Mixter.Domain.Identity

open System

type UserId = UserId of string

type SessionId = SessionId of string
    with static member generate = SessionId (Guid.NewGuid().ToString())
        
type Event = 
    | UserRegistered of UserRegisteredEvent
    | UserConnected of UserConnectedEvent
    | UserDisconnected of UserDisconnectedEvent
and UserRegisteredEvent = { UserId: UserId }
and UserConnectedEvent = { SessionId: SessionId; UserId: UserId; ConnectedAt: DateTime}
and UserDisconnectedEvent = { SessionId: SessionId; UserId: UserId }

type DecisionProjection = 
    | UnregisteredUser
    | RegisteredUser of RegisteredUser
    | ConnectedUser of ConnectedUser
and RegisteredUser = { UserId: UserId }
and ConnectedUser = { UserId: UserId; SessionId: SessionId }

let register userId =
    [ UserRegistered { UserId = userId } ]

let logIn sessionId getCurrentTime decisionProjection =
    match decisionProjection with
    | RegisteredUser p -> [ UserConnected { SessionId = sessionId; UserId = p.UserId; ConnectedAt = getCurrentTime () } ]
    | _ -> []

let logOut decisionProjection =
    match decisionProjection with
    | ConnectedUser p -> [ UserDisconnected { SessionId = p.SessionId; UserId = p.UserId } ]
    | _ -> []

let applyOne decisionProjection event =
    match (decisionProjection, event) with
    | (UnregisteredUser, UserRegistered e) -> RegisteredUser { UserId = e.UserId }
    | (RegisteredUser _, UserConnected e) -> ConnectedUser { UserId = e.UserId; SessionId = e.SessionId }
    | (ConnectedUser _, UserDisconnected e) -> RegisteredUser { UserId = e.UserId }
    | _ -> failwith "Invalid transition"

let apply events =
    Seq.fold applyOne UnregisteredUser events

module Read =
    type Session = { SessionId: SessionId; UserId: UserId }
        with static member empty = { SessionId = SessionId ""; UserId = UserId "" }
        
    type RepositoryChange = 
        | None
        | Add of Session
        | Remove of SessionId

    type getSessionById = SessionId -> Session option

    let apply (getSessionById:getSessionById) event = 
        match event with
        | UserConnected e -> Add { SessionId = e.SessionId; UserId = e.UserId }
        | UserDisconnected e -> Remove e.SessionId
        | UserRegistered _ -> None