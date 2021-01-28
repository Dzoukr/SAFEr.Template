#r "paket: groupref Build //"
#load ".fake/build.fsx/intellisense.fsx"

open System.IO
open Fake.Core
open Fake.IO
open Fake.DotNet
open Fake.IO.Globbing.Operators
open Fake.IO.FileSystemOperators
open Fake.Core.TargetOperators

module Tools =
    let private findTool tool winTool =
        let tool = if Environment.isUnix then tool else winTool
        match ProcessUtils.tryFindFileOnPath tool with
        | Some t -> t
        | _ ->
            let errorMsg =
                tool + " was not found in path. " +
                "Please install it and make sure it's available from your path. "
            failwith errorMsg

    let private runTool (cmd:string) args workingDir =
        let arguments = args |> String.split ' ' |> Arguments.OfArgs
        Command.RawCommand (cmd, arguments)
        |> CreateProcess.fromCommand
        |> CreateProcess.withWorkingDirectory workingDir
        |> CreateProcess.ensureExitCode
        |> Proc.run
        |> ignore

    let dotnet cmd workingDir =
        let result =
            DotNet.exec (DotNet.Options.withWorkingDirectory workingDir) cmd ""
        if result.ExitCode <> 0 then failwithf "'dotnet %s' failed in %s" cmd workingDir

    let femto = runTool "femto"
    let node = runTool (findTool "node" "node.exe")
    let yarn = runTool (findTool "yarn" "yarn.cmd")

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
    Tools.dotnet (sprintf "fable --outDir %s --run webpack-cli -p" fableBuildPath) clientSrcPath
)

Target.create "PublishInfrastructure" (fun _ ->
    Directory.ensure infrastructurePublishPath
    "Infrastructure.fsx" |> Shell.copyFile infrastructurePublishPath
)

Target.create "Run" (fun _ ->
    let server = async {
        Tools.dotnet "watch run" serverSrcPath
    }
    let client = async {
        Tools.dotnet (sprintf "fable watch --outDir %s --run webpack-dev-server" fableBuildPath) clientSrcPath
    }
    [server;client]
    |> Async.Parallel
    |> Async.RunSynchronously
    |> ignore
)

"InstallClient"
    ==> "PublishInfrastructure"
    ==> "Publish"

"InstallClient"
    ==> "Run"

Target.runOrDefaultWithArguments "Run"