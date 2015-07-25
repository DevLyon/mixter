module Mixter.Domain.Message

open Mixter.Domain.Identity
open System

type MessageId = MessageId of string
    with static member generate = MessageId (Guid.NewGuid().ToString())

type Event =
    | MessagePublished of MessagePublished
    | MessageRepublished of MessageRepublished
and MessagePublished = { MessageId: MessageId; UserId: UserId; Content: string}
and MessageRepublished = { MessageId: MessageId }

type DecisionProjection = 
    | InitialProjection
    | MessagePublishedProjection of MessagePublishedProjection 
and MessagePublishedProjection = { MessageId: MessageId; AuthorId: UserId; }

let publish messageId authorId content =
    [ MessagePublished { MessageId = messageId; UserId = authorId; Content = content } ]

let republish republisherId decisionProjection =
    match decisionProjection with
    | MessagePublishedProjection p when p.AuthorId <> republisherId -> [ MessageRepublished { MessageId = p.MessageId } ]
    | MessagePublishedProjection _
    | InitialProjection -> []

let applyOne decisionProjection event =
    match event with
    | MessagePublished e -> MessagePublishedProjection { MessageId = e.MessageId; AuthorId = e.UserId }
    | MessageRepublished _ -> decisionProjection

let apply decisionProjection =
    Seq.fold applyOne decisionProjection