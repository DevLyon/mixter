module Mixter.Domain.Core.Message

open Mixter.Domain.Documentation
open Mixter.Domain.Identity.UserIdentity
open System

type MessageId = MessageId of string
    with static member Generate() = MessageId (Guid.NewGuid().ToString())

[<Event>]
type Event =
    | MessageQuacked of MessageQuacked
    | MessageRequacked of MessageRequacked
    | MessageDeleted of MessageDeleted
and MessageQuacked = { MessageId: MessageId; AuthorId: UserId; Content: string}
and MessageRequacked = { MessageId: MessageId; Requacker: UserId }
and MessageDeleted = { MessageId: MessageId; Deleter: UserId }

[<Projection>]
type DecisionProjection = 
    | NotQuackedMessage
    | QuackedMessage of QuackedMessage
    | DeletedMessage
and QuackedMessage = { MessageId: MessageId; AuthorId: UserId; Requackers: UserId list }

let applyOne decisionProjection = function
    | MessageQuacked e -> 
        QuackedMessage { 
            MessageId = e.MessageId
            AuthorId = e.AuthorId
            Requackers = [] 
        }

    | MessageRequacked e -> 
        match decisionProjection with
        | QuackedMessage p -> QuackedMessage { p with Requackers = e.Requacker :: p.Requackers }
        | _ -> decisionProjection
        
    | MessageDeleted _ -> 
        match decisionProjection with
        | QuackedMessage _ -> DeletedMessage
        | _ -> decisionProjection

let apply events =
    Seq.fold applyOne NotQuackedMessage events

[<Command>]
let quack messageId authorId content =
    [ MessageQuacked { MessageId = messageId; AuthorId = authorId; Content = content } ]

[<Command>]
let requack requackerId history =
    match history |> apply with
    | QuackedMessage p when p.AuthorId = requackerId -> []
    | QuackedMessage p when p.Requackers |> List.exists (fun r -> r = requackerId) -> []
    | QuackedMessage p -> [ MessageRequacked { MessageId = p.MessageId; Requacker = requackerId } ]
    | _ -> []

[<Command>]
let delete deleter history = 
    match history |> apply with
    | QuackedMessage p when p.AuthorId <> deleter -> []
    | QuackedMessage p -> [ MessageDeleted { MessageId = p.MessageId; Deleter = deleter } ]
    | _ -> []
