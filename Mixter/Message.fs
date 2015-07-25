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
    | NotPublishedMessage
    | PublishedMessage of PublishedMessage 
and PublishedMessage = { MessageId: MessageId; AuthorId: UserId; }

let publish messageId authorId content =
    [ MessagePublished { MessageId = messageId; UserId = authorId; Content = content } ]

let republish republisherId decisionProjection =
    match decisionProjection with
    | PublishedMessage p when p.AuthorId <> republisherId -> [ MessageRepublished { MessageId = p.MessageId } ]
    | PublishedMessage _
    | NotPublishedMessage -> []

let applyOne decisionProjection event =
    match event with
    | MessagePublished e -> PublishedMessage { MessageId = e.MessageId; AuthorId = e.UserId }
    | MessageRepublished _ -> decisionProjection

let apply events =
    Seq.fold applyOne NotPublishedMessage events