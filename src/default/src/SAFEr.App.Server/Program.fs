module SAFEr.App.Server.Program

open System
open Giraffe
open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.Logging
open Microsoft.AspNetCore.Hosting

[<RequireQualifiedAccess>]
module Configuration =
    let load () =
            (ConfigurationBuilder())
    #if DEBUG
                .AddJsonFile("appsettings.development.json", true)
    #endif
                .AddEnvironmentVariables()
                .Build()

let configureApp (cfg:IConfigurationRoot) (app:IApplicationBuilder) =
    app
        .UseStaticFiles()
        .UseGiraffe WebApp.webApp

let configureServices (cfg:IConfigurationRoot) (services:IServiceCollection) =
    services
        .AddApplicationInsightsTelemetry(cfg.["APPINSIGHTS_INSTRUMENTATIONKEY"])
        .AddGiraffe() |> ignore

[<EntryPoint>]
let main _ =
    let config = Configuration.load()

    Host.CreateDefaultBuilder()
        .ConfigureWebHostDefaults(
            fun webHostBuilder ->
                webHostBuilder
                    .Configure(configureApp config)
                    .ConfigureServices(configureServices config)
                    .ConfigureLogging(fun x ->
                        x.AddFilter<Microsoft.Extensions.Logging.ApplicationInsights.ApplicationInsightsLoggerProvider> ("", LogLevel.Information) |> ignore
                    )
                    .UseUrls([|"http://0.0.0.0:8085"|])
                    .UseWebRoot("public")
                    |> ignore)
        .Build()
        .Run()
    0