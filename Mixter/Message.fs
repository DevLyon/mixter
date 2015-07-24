namespace Mixter.Domain

open Mixter.Domain.Identity

module Message =
    open System

    type MessageId = MessageId of string
        with static member generate = MessageId (Guid.NewGuid().ToString())

    type Event =
        | MessagePublished of MessagePublished
        | MessageRepublished of MessageRepublished
    and MessagePublished = { MessageId: MessageId; UserId: UserId; Content: string}
    and MessageRepublished = { MessageId: MessageId }

    type DecisionProjection = {
        MessageId: MessageId option
    }
        with static member initial = { MessageId = None }

    let publish messageId userId content =
        [ MessagePublished { MessageId = messageId; UserId = userId; Content = content } ]

    let republish decisionProjection =
        [ MessageRepublished { MessageId = decisionProjection.MessageId.Value } ]

    let applyOne decisionProjection event =
        match event with
        | MessagePublished e -> { decisionProjection with MessageId = Some e.MessageId }
        | _ -> failwith "Unknown event"

    let apply decisionProjection events =
        Seq.fold applyOne decisionProjection events