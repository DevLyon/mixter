#load "Identity.fs"
#load "Identity.Infrastructure.fs"

open System

open Mixter
open Domain.Identity

// Simulate user registration
let userId = UserId "clem@mix-it.fr"
let registrationEvents = register userId

// Simulate user reloading & logIn
let user = apply registrationEvents
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
    |> Seq.iter (Infrastructure.Identity.Read.applyChangeInMemory sessionProjections)

// Read session projection
let session = getSessionById sessionId
