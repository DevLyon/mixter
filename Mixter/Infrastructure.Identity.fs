module Mixter.Infrastructure.Identity

open Mixter.Domain.Documentation
open Mixter.Domain.Identity.Session
open Mixter.Domain.Identity.SessionDescription

open System.Collections.Generic

[<Repository>]
type MemorySessionsStore() =
    let store = Dictionary<SessionId, SessionDescription>()

    [<Query>]
    member _.GetSession sessionId = 
        if store.ContainsKey(sessionId) 
        then Some store[sessionId]
        else option.None

    [<Query>]
    member _.GetUserSession userId =
        let keyValue = store |> Seq.tryFind (fun d -> d.Value.UserId = userId)
        match keyValue with
        | Some x -> Some x.Key
        | None -> None

    member _.Add sessionId session = 
        store.Add (sessionId, session)

    member _.Remove sessionId = 
        store.Remove sessionId |> ignore
