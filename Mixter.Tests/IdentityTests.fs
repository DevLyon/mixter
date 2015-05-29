namespace Mixter.Domain.Identity.Tests

open NUnit.Framework
open FsUnit
open Mixter.Domain.Identity

[<TestFixture>]
type ``User aggregate`` ()=

    [<Test>] 
    member x.``When a user register, then user registered event is published`` () =
        let assertEvents = should equal [ UserRegistered { UserId = UserId "clem@mix-it.fr" } ]
        let assertUserRegistedPublished aggregateId events = events |> assertEvents
        register assertUserRegistedPublished (UserId "clem@mix-it.fr" ) |> assertEvents
