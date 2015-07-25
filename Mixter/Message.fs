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

type DecisionProjection = {
    MessageId: MessageId option;
    AuthorId: UserId option;
}
    with static member initial = { MessageId = None; AuthorId = None }

let publish messageId authorId content =
    [ MessagePublished { MessageId = messageId; UserId = authorId; Content = content } ]

let republish republisherId decisionProjection =
    if republisherId = decisionProjection.AuthorId.Value
    then []
    else [ MessageRepublished { MessageId = decisionProjection.MessageId.Value } ]

let applyOne decisionProjection event =
    match event with
    | MessagePublished e -> { decisionProjection with MessageId = Some e.MessageId; AuthorId = Some e.UserId }
    | MessageRepublished _ -> decisionProjection

let apply decisionProjection =
    Seq.fold applyOne decisionProjection