open System.IO
open System.Text.RegularExpressions
open Fake.Core
open Fake.IO
open Fake.Core.TargetOperators

open BuildHelpers
open BuildTools

initializeContext()

Target.create "Clean" (fun _ -> [".nupkg"] |> Shell.cleanDirs)

Target.create "Publish" (fun _ -> 
    let nugetKey =
        match Environment.environVarOrNone "NUGET_KEY" with
        | Some nugetKey -> nugetKey
        | None -> failwith "The Nuget API key must be set in a NUGET_KEY environmental variable"
    let nupkg =
        ".nupkg"
        |> Directory.GetFiles
        |> Seq.head
        |> Path.GetFullPath
    Tools.dotnet (sprintf "nuget push %s -s nuget.org -k %s" nupkg nugetKey) __SOURCE_DIRECTORY__
)

Target.create "Pack" (fun _ -> Tools.dotnet "pack SAFEr.Template.proj -c Release -o .nupkg" __SOURCE_DIRECTORY__)

Target.create "SetVersion" (fun _ ->
    let version =
        "SAFEr.Template.proj"
        |> File.readAsString
        |> (fun x -> Regex.Match (x, "<Version>(.*)</Version>"))
        |> (fun x -> x.Groups.[1].Value)
    
    Shell.regexReplaceInFileWithEncoding
        "  \"name\": .+,"
       ("  \"name\": \"SAFEr Web App v" + version + "\",")
        System.Text.Encoding.UTF8
        "src/.template.config/template.json"
    ()
)

let dependencies = [
    "Clean" ==> "SetVersion" ==> "Pack" ==> "Publish"
]

[<EntryPoint>]
let main args = runOrDefault args