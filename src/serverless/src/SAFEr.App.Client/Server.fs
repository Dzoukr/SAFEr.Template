module SAFEr.App.Client.Server

open Fable.Core
open Fable.Remoting.Client
open SAFEr.App.Shared.API
open Fable.SimpleJson
open SAFEr.App.Shared.Errors

[<Emit("config.baseUrl")>]
let baseUrl : string = jsNative

let private exnToError (e:exn) : ServerError =
    match e with
    | :? ProxyRequestException as ex ->
        try
            let serverError = Json.parseAs<{| error: ServerError |}>(ex.Response.ResponseBody)
            serverError.error
        with _ -> ServerError.Exception(e.Message)
    | _ -> ServerError.Exception(e.Message)

type ServerResult<'a> = Result<'a,ServerError>

module Cmd =
    open Elmish

    module OfAsync =
        let eitherAsResult fn resultMsg =
            Cmd.OfAsync.either fn () (Result.Ok >> resultMsg) (exnToError >> Result.Error >> resultMsg)

let service =
    Remoting.createApi()
    |> Remoting.withBaseUrl baseUrl
    |> Remoting.withRouteBuilder Service.RouteBuilder
    |> Remoting.buildProxy<Service>