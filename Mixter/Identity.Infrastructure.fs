module Mixter.Infrastructure.Identity.Read

open Mixter.Domain.Identity
open Mixter.Domain.Identity.Read

open System.Collections.Generic

let getSessionByIdFromMemory (sessions:Dictionary<SessionId, Session>) sessionId = 
    let mutable session = Session.empty
    if sessions.TryGetValue(sessionId, &session) 
    then Some session
    else option.None
    
let applyChangeInMemory (sessions:Dictionary<SessionId, Session>) change = 
    match change with 
    | Add session -> sessions.Add (session.SessionId, session)
    | Remove session -> sessions.Remove session.SessionId |> ignore
    | None -> ()