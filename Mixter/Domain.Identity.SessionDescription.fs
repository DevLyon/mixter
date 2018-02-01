module Mixter.Domain.Identity.SessionDescription

open Mixter.Domain.Documentation
open Mixter.Domain.Identity.UserIdentity
open Mixter.Domain.Identity.Session

[<Projection>]
type SessionDescription = { UserId: UserId }

[<Handler>]
let handle add remove event = 
    match event with
    | UserConnected e -> 
        add e.SessionId { UserId = e.UserId }
    | UserDisconnected e -> 
        remove e.SessionId
