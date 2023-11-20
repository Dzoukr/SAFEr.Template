module SAFEr.App.Server.Program

open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.Hosting
open Giraffe

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
    |> configureWeb

let app =
    builder.Build()
    |> configureApp

app.Run()