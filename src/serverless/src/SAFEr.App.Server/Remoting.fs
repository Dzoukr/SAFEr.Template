namespace Fable.Remoting.Server

open System
open Fable.Remoting.Server
open Microsoft.Azure.Functions.Worker.Http
open SAFEr.App.Shared.Errors
open Microsoft.Extensions.Logging

[<RequireQualifiedAccess>]
module Remoting =
    let rec errorHandler (log:ILogger) (ex: Exception) (routeInfo: RouteInfo<HttpRequestData>) =
        log.LogError(ex, ex.Message)
        match ex with
        | ServerException err -> Propagate err
        | e when e.InnerException |> isNull |> not -> errorHandler log e.InnerException routeInfo
        | _ -> Propagate (ServerError.Exception(ex.Message))