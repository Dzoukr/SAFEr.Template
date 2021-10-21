open Fake
open Fake.Core
open Fake.IO
open Fake.IO.FileSystemOperators
open Fake.Core.TargetOperators

open BuildHelpers
open BuildTools

initializeContext()

let publishPath = Path.getFullName "publish"
let srcPath = Path.getFullName "src"
let clientSrcPath = srcPath </> "SAFEr.App.Client"
let serverSrcPath = srcPath </> "SAFEr.App.Server"
let appPublishPath = publishPath </> "app"
let fableBuildPath = clientSrcPath </> ".fable-build"
let infrastructurePublishPath = publishPath </> "infrastructure"

// Targets
let clean proj = [ proj </> "bin"; proj </> "obj" ] |> Shell.cleanDirs

Target.create "InstallClient" (fun _ ->
    printfn "Node version:"
    Tools.node "--version" clientSrcPath
    printfn "Yarn version:"
    Tools.yarn "--version" clientSrcPath
    Tools.yarn "install --frozen-lockfile" clientSrcPath
)

Target.create "Publish" (fun _ ->
    [ appPublishPath ] |> Shell.cleanDirs
    let publishArgs = sprintf "publish -c Release -o \"%s\"" appPublishPath
    Tools.dotnet publishArgs serverSrcPath
    [ appPublishPath </> "appsettings.Development.json" ] |> File.deleteAll
    Tools.yarn "build" ""
)

Target.create "PublishInfrastructure" (fun _ ->
    Directory.ensure infrastructurePublishPath
    "Infrastructure.fsx" |> Shell.copyFile infrastructurePublishPath
)

Target.create "Run" (fun _ ->
    let server = async {
        Environment.setEnvironVar "ASPNETCORE_ENVIRONMENT" "Development"
        Tools.dotnet "watch run" serverSrcPath
    }
    let client = async {
        Tools.yarn "start" ""
    }
    [server;client]
    |> Async.Parallel
    |> Async.RunSynchronously
    |> ignore
)

let dependencies = [
    "InstallClient"
        ==> "PublishInfrastructure"
        ==> "Publish"

    "InstallClient"
        ==> "Run"
]

[<EntryPoint>]
let main args = runOrDefault args