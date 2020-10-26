module SAFEr.App.Server.WebApp

open Giraffe
open System.IO
open Fable.Remoting.Server
open Fable.Remoting.Giraffe
open SAFEr.App.Shared.Communication
open FSharp.Control.Tasks.V2

let service = {
    GetMessage = fun _ -> task { return "Hi from Server" } |> Async.AwaitTask
}

let webApp : HttpHandler =
    let remoting =
        Remoting.createApi()
        |> Remoting.withRouteBuilder Service.RouteBuilder
        |> Remoting.fromValue service
        |> Remoting.buildHttpHandler
    choose [
        remoting
        htmlFile "public/index.html"
    ]