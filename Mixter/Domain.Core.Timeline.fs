module Mixter.Domain.Core.Timeline

open Mixter.Domain.Documentation
open Mixter.Domain.Identity.UserIdentity
open Mixter.Domain.Core.Message

[<Projection>]
type TimelineMessage = { Owner: UserId; Author: UserId; Content: string; MessageId: MessageId }
