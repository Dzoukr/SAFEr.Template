module SAFEr.App.Infrastructure.Program

open System
open Fake.Core
open Farmer
open Deployments

// CLI interface definition
let cli = """
usage: deploy [options]

options:
 -f --folder <path>                  Deploy app from
 -g --resourcegroup <name>           Resource group name
"""

// helper module to read easily required/optional arguments
module DocoptResult =
    let getArgumentOrDefault arg def map =
        map
        |> DocoptResult.tryGetArgument arg
        |> Option.defaultValue def

    let getArgument arg map =
        map
        |> DocoptResult.tryGetArgument arg
        |> Option.defaultWith (fun _ ->
            failwithf "Missing required argument %s.%sPlease use this CLI interface: %s" arg Environment.NewLine cli)

let ignoreOk = function
    | Ok _ -> ()
    | Error e -> failwithf "An error occured: %A" e

[<EntryPoint>]
let main argv =
    let cliArgs = Docopt(cli).Parse(argv)
    let appFolder = cliArgs |> DocoptResult.getArgument "-f"
    let group = cliArgs |> DocoptResult.getArgument "-g"

    appFolder
    |> getDefault
    |> Deploy.execute group Deploy.NoParameters
    |> ignore
    0