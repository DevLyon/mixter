namespace Mixter.Domain

module Identity =
    open System

    type UserId = UserId of string

    type SessionId = SessionId of string
        with static member generate = SessionId (Guid.NewGuid().ToString())

    type UserRegisteredEvent = { UserId: UserId }

    type UserConnectedEvent = { SessionId: SessionId; UserId: UserId; ConnectedAt: DateTime}

    type Event = 
        UserRegistered of UserRegisteredEvent
        | UserConnected of UserConnectedEvent

    type DecisionProjection = {
        UserId: UserId
    }
        with static member initial = { UserId = UserId "" }

    let register userId =
        [ UserRegistered { UserId = userId } ]

    let logIn sessionId getCurrentTime decisionProjection =
        [ UserConnected { SessionId = sessionId; UserId = decisionProjection.UserId; ConnectedAt = getCurrentTime () } ]

    let apply decisionProjection event =
        match event with
        | UserRegistered e -> { UserId = e.UserId }
        | UserConnected _ -> decisionProjection

    