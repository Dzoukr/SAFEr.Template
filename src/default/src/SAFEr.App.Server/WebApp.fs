module SAFEr.App.Server.WebApp

open Giraffe
open Giraffe.GoodRead
open Fable.Remoting.Server
open Fable.Remoting.Giraffe
open Microsoft.Extensions.Logging
open SAFEr.App.Shared.API
open SAFEr.App.Shared.Errors

let service = {
    GetMessage = fun success ->
        task {
            if success then return "Hi from Server!"
            else return ServerError.failwith (ServerError.Exception "OMG, something terrible happened")
        }
        |> Async.AwaitTask
}

let webApp : HttpHandler =
    let remoting logger =
        Remoting.createApi()
        |> Remoting.withRouteBuilder Service.RouteBuilder
        |> Remoting.fromValue service
        |> Remoting.withErrorHandler (Remoting.errorHandler logger)
        |> Remoting.buildHttpHandler
    choose [
        Require.services<ILogger<_>> remoting
        htmlFile "public/index.html"
    ]