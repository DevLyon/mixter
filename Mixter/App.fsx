#load "Identity.fs"
#load "Identity.Infrastructure.fs"

open System

open Mixter
open Domain.Identity

// Simulate user registration
let registrationEvents = 
    UserId "clem@mix-it.fr" 
        |> register

// Simulate user reloading & logIn
let now = fun () -> DateTime.Now
let sessionId = SessionId.generate
let loginEvents = 
    registrationEvents
        |> apply
        |> logIn sessionId now

// Simulate session projection
open Mixter.Infrastructure.Identity.Read
open System.Collections.Generic
// Use a Dictionary instead of Map
let sessionProjections = new MemorySessionsStore()
loginEvents 
    |> Seq.map (Read.project sessionProjections.GetSession)
    |> Seq.iter sessionProjections.ApplyChange

// Read session projection
let session = sessionProjections.GetSession sessionId
