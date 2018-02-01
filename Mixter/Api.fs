module Mixter.Api

open Suave
open Suave.Filters
open Suave.Operators
open Suave.Successful
open Mixter.Domain.Identity
open Mixter.Infrastructure
open Newtonsoft.Json

let eventsStore = EventsStore.MemoryEventsStore()

type RegisterResult = { Id: string; Url: string; LogIn: string }
type RegisterDto = { Email: string } 

let register (dto: RegisterDto) = 
    let userId: UserIdentity.UserId = { Email = dto.Email }
    UserIdentity.register userId
    |> eventsStore.Store userId

    let userIdFormatted = userId |> sprintf "%A"
    {
        Id = userIdFormatted
        Url = "/api/identity/userIdentities/" + System.Uri.EscapeUriString(userIdFormatted)
        LogIn = "/api/identity/userIdentities/" + System.Uri.EscapeUriString(userIdFormatted) + "/logIn"
    }

let serialize obj : string =
    JsonConvert.SerializeObject(obj)

let deserialize<'a> (param: HttpRequest) =
    JsonConvert.DeserializeObject<'a>(param.rawForm |> System.Text.Encoding.UTF8.GetString)

let requestWrapper (handler: 'a -> 'b) =
    request(fun r -> r |> deserialize<'a> |> handler |> serialize |> OK)

let app =
  choose [ 
    path "/api/identity/userIdentities/register" >=> choose [
      POST >=> requestWrapper register ]
    RequestErrors.NOT_FOUND "Found no handlers" ]

let start () =
    startWebServer defaultConfig app

