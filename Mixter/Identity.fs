namespace Mixter.Domain

module Identity =
    open System

    type UserId = UserId of string

    type SessionId = SessionId of string

    type UserRegisteredEvent = { UserId: UserId }
    
    type Event = 
        UserRegistered of UserRegisteredEvent

    let register publish userId =
        let userRegistered = UserRegistered { UserId = userId }
        publish userId [ userRegistered ]
        [ userRegistered ]
