module Mixter.Tests.Domain.Identity.UserIdentity

open Xunit
open Swensen.Unquote
open Mixter.Domain.Identity.UserIdentity;

module ``UserIdentity should`` =

    [<Fact>] 
    let ``return UserRegistered When register`` () =
        test <@ register ({ Email = "user@mix-it.fr" }) 
                    = [ UserRegistered { UserId = { Email = "user@mix-it.fr"} } ] @>