#load "Identity.fs"
#load "Identity.Infrastructure.fs"

open System

open Mixter
open Domain.Identity
open Infrastructure.Identity.Read

let simulateUserRegistration = 
    UserId "clem@mix-it.fr" 
        |> register

let simulateUserLogin userEvents =
    let now = fun () -> DateTime.Now
    let sessionId = SessionId.generate
    
    let newEvents = 
        userEvents
            |> apply
            |> logIn sessionId now

    (newEvents, sessionId)
        
let sessionsStore = new MemorySessionsStore()

let simulateSessionStorage (userEvents, sessionId) =
    userEvents 
        |> Seq.map (Read.apply sessionsStore.GetSession)
        |> Seq.iter sessionsStore.ApplyChange

    sessionId

simulateUserRegistration
    |> simulateUserLogin
    |> simulateSessionStorage
    |> sessionsStore.GetSession