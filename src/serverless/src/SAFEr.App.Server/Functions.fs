namespace SAFEr.App.Server

open Fable.Remoting.Server
open Fable.Remoting.AzureFunctions.Worker
open Microsoft.Azure.Functions.Worker
open Microsoft.Azure.Functions.Worker.Http
open Microsoft.Extensions.Logging
open SAFEr.App.Shared.API

type Functions(log:ILogger<Functions>) =
    let service = {
        GetMessage = fun _ -> task { return "Hi from Azure Functions!" } |> Async.AwaitTask
    }

    [<Function("Index")>]
    member _.Index ([<HttpTrigger(AuthorizationLevel.Anonymous, Route = "{*any}")>] req: HttpRequestData, ctx: FunctionContext) =
        Remoting.createApi()
        |> Remoting.withRouteBuilder FunctionsRouteBuilder.apiPrefix
        |> Remoting.fromValue service
        |> Remoting.buildRequestHandler
        |> HttpResponseData.fromRequestHandler req