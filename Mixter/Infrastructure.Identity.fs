module Mixter.Infrastructure.Identity

open Mixter.Domain.Documentation
open Mixter.Domain.Identity.Session
open Mixter.Domain.Identity.SessionDescription

open System.Collections.Generic

[<Repository>]
type MemorySessionsStore() =
    let store = new Dictionary<SessionId, SessionDescription>()

    [<Query>]
    member __.GetSession sessionId = 
        if store.ContainsKey(sessionId) 
        then Some store.[sessionId]
        else option.None

    [<Query>]
    member __.GetUserSession userId =
        let keyValue = store |> Seq.tryFind (fun d -> d.Value.UserId = userId)
        match keyValue with
        | Some x -> Some x.Key
        | None -> None

    member __.Add sessionId session = 
        store.Add (sessionId, session)

    member __.Remove sessionId = 
        store.Remove sessionId |> ignore
