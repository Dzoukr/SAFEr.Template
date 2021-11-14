#r "nuget: Farmer"
#r "nuget: Fake.Core.CommandLineParsing"

open System
open Farmer
open Fake.Core

module Deployments =

    open Farmer.Builders

    let get (appPath:string) =
        // appinsights
        let insights = appInsights {
            name "appinsights-SAFEr.App"
        }

        // web app
        let web = functions {
            name "web-SAFEr.App"
            always_on
            https_only
            link_to_app_insights insights.Name
            zip_deploy appPath
        }

        arm {
            location Location.WestEurope
            add_resource insights
            add_resource web
        }

[<RequireQualifiedAccess>]
module DocoptResult =
    let getArgument arg map =
        map
        |> DocoptResult.tryGetArgument arg
        |> Option.defaultWith (fun _ -> failwithf "Missing required argument %s." arg)

// CLI interface definition
let cli =
    """
USAGE:
    infrastructure.fsx (-h | --help)
    infrastructure.fsx <folder> <resourcegroup> [options]
OPTIONS [options]:
    -h --help                          Shows help
"""

try
    let args =
        fsi.CommandLineArgs
        |> List.ofArray
        |> function
            | [] -> []
            | l -> l |> List.tail

    let cliArgs = Docopt(cli).Parse(args)
    if (cliArgs |> DocoptResult.hasFlag "-h")
    then
        printfn "%s" cli
        0
    else
        let folder = cliArgs |> DocoptResult.getArgument "<folder>"
        let resourceGroup = cliArgs |> DocoptResult.getArgument "<resourcegroup>"

        try
            folder
            |> Deployments.get
            |> Deploy.execute resourceGroup Deploy.NoParameters
            |> ignore

            Console.ForegroundColor <- ConsoleColor.Green
            printfn "Deploy from '%s' to resource group '%s' was successfull" folder resourceGroup
            0
        with ex ->
            Console.ForegroundColor <- ConsoleColor.Red
            eprintfn "Exception occured: %s" ex.Message
            1
with ex ->
    Console.ForegroundColor <- ConsoleColor.Red
    printfn "Error while parsing command line, usage is:"
    printfn "%s" cli
    eprintfn "Exception occured: %s" ex.Message
    1