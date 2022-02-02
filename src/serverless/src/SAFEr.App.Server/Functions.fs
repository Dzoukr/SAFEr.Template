namespace SAFEr.App.Server

open Fable.Remoting.Server
open Fable.Remoting.AzureFunctions.Worker
open Microsoft.Azure.Functions.Worker
open Microsoft.Azure.Functions.Worker.Http
open Microsoft.Extensions.Logging
open SAFEr.App.Shared.API
open SAFEr.App.Shared.Errors

type Functions(log:ILogger<Functions>) =
    let service = {
        GetMessage = fun success ->
            task {
                if success then return "Hi from Azure Functions!"
                else return ServerError.failwith (ServerError.Exception "OMG, something terrible happened")
            }
            |> Async.AwaitTask
    }

    [<Function("Index")>]
    member _.Index ([<HttpTrigger(AuthorizationLevel.Anonymous, Route = "{*any}")>] req: HttpRequestData, ctx: FunctionContext) =
        Remoting.createApi()
        |> Remoting.withRouteBuilder FunctionsRouteBuilder.apiPrefix
        |> Remoting.fromValue service
        |> Remoting.withErrorHandler (Remoting.errorHandler log)
        |> Remoting.buildRequestHandler
        |> HttpResponseData.fromRequestHandler req