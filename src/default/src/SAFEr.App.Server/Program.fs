module SAFEr.App.Server.Program

open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.Logging
open Microsoft.Extensions.DependencyInjection
open Giraffe

let private configureLogging (builder: WebApplicationBuilder) =
    builder.Host.ConfigureLogging(fun x ->
        x.AddFilter<Microsoft.Extensions.Logging.ApplicationInsights.ApplicationInsightsLoggerProvider> ("", LogLevel.Information) |> ignore
    ) |> ignore
    builder

let private addApplicationInsights (builder:WebApplicationBuilder) =
    builder.Services.AddApplicationInsightsTelemetry() |> ignore
    builder

let private configureWeb (builder:WebApplicationBuilder) =
    builder.Services.AddGiraffe() |> ignore
    builder

let private configureApp (app:WebApplication) =
    app.UseStaticFiles() |> ignore
    app.UseGiraffe WebApp.webApp
    app

let private builderOptions = WebApplicationOptions(WebRootPath = "public")
let private builder =
    WebApplication.CreateBuilder(builderOptions)
    |> addApplicationInsights
    |> configureLogging
    |> configureWeb

let app =
    builder.Build()
    |> configureApp

app.Run()