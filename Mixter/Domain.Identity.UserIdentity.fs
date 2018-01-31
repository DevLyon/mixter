module Mixter.Domain.Identity.UserIdentity

open Mixter.Domain.Documentation

[<StructuredFormatDisplay("{Email}")>]
type UserId = { Email: string }

[<Event>]
type Event = 
    | UserRegistered of UserRegistered
and UserRegistered = { UserId: UserId }

[<Command>]
let register userId = 
    [ UserRegistered { UserId = userId } ]
