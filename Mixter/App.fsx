#load "Identity.fs"
#load "Identity.Infrastructure.fs"

open Mixter.Domain.Identity
open System

// Simulate user registration
let userId = UserId "clem@mix-it.fr"
let registrationEvents = register userId

// Simulate user reloading & logIn
let user = apply UnregisteredUser registrationEvents
let now = fun () -> DateTime.Now
let sessionId = SessionId.generate
let loginEvents = logIn sessionId now user

// Simulate session projection
open Mixter.Infrastructure.Identity.Read
open System.Collections.Generic
// Use a Dictionary instead of Map
let sessionProjections = new Dictionary<SessionId, Read.Session>()
let getSessionById = getSessionByIdFromMemory sessionProjections
loginEvents 
    |> Seq.map (Read.project getSessionById)
    |> Seq.iter (Mixter.Infrastructure.Identity.Read.applyChangeInMemory sessionProjections)

// Read session projection
let session = getSessionById sessionId
