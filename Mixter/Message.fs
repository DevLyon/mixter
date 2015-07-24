namespace Mixter.Domain

open Mixter.Domain.Identity

module Message =
    open System

    type MessageId = MessageId of string
        with static member generate = MessageId (Guid.NewGuid().ToString())

    type Event =
        | MessagePublished of MessagePublished
    and MessagePublished = { MessageId: MessageId; UserId: UserId; Content: string}

    let publish messageId userId content =
        [ MessagePublished { MessageId = messageId; UserId = userId; Content = content } ]