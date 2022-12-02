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

// Targets
let clean proj = [ proj </> "bin"; proj </> "obj" ] |> Shell.cleanDirs

Target.create "InstallClient" (fun _ ->
    printfn "Node version:"
    run Tools.node "--version" clientSrcPath
    printfn "Yarn version:"
    run Tools.yarn "--version" clientSrcPath
    run Tools.yarn "install --frozen-lockfile" clientSrcPath
)

Target.create "Publish" (fun _ ->
    [ publishPath ] |> Shell.cleanDirs
    let publishArgs = sprintf "publish -c Release -o \"%s\"" appPublishPath
    run Tools.dotnet publishArgs serverSrcPath
    [ appPublishPath </> "local.settings.json" ] |> File.deleteAll
    run Tools.dotnet "fable clean --yes" ""
    run Tools.yarn "build" ""
)

Target.create "Run" (fun _ ->
    run Tools.dotnet "fable clean --yes" ""
    [
        "server", Tools.dotnet "watch msbuild /t:RunFunctions" serverSrcPath
        "client", Tools.yarn "start" ""
    ]
    |> runParallel
)

let dependencies = [
    "InstallClient"
        ==> "Publish"
    "InstallClient"
        ==> "Run"
]

[<EntryPoint>]
let main args = runOrDefault "Run" args