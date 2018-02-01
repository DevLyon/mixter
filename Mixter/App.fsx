#r "../packages/Suave/lib/net40/Suave.dll"
#r "../packages/Newtonsoft.Json/lib/portable-net45+win8+wp8+wpa81/Newtonsoft.Json.dll"

#load "Domain.Identity.UserIdentity.fs"
#load "Domain.Identity.Session.fs"
#load "Domain.Identity.SessionDescription.fs"
#load "Infrastructure.Identity.fs"
#load "Infrastructure.EventsStore.fs"
#load "Api.fs"

open System

open Mixter
open Mixter.Domain.Identity
open UserIdentity
open Session
open Infrastructure.Identity
open Infrastructure.EventsStore

let sessionsStore = MemorySessionsStore()
let eventsStore = MemoryEventsStore()

let sessionHandler (event: Session.Event) =
    SessionDescription.handle sessionsStore.Add sessionsStore.Remove event

let userIdentityHandler (event: UserIdentity.Event) =
    ()

let publish handler aggregateId events = 
    eventsStore.Store aggregateId events
    events |> Seq.iter handler

let simulateUserRegistration = 
    let userId = { Email = "clem@mix-it.fr" }
    userId
    |> register
    |> publish userIdentityHandler userId

    userId

let simulateUserLogin userId =
    let sessionId = SessionId.Generate()
    
    logIn userId sessionId DateTime.Now
    |> publish sessionHandler sessionId

let userId = simulateUserRegistration
simulateUserLogin userId
let sessionId = sessionsStore.GetUserSession userId

Api.start()