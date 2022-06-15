module Feliz.UseElmish

(*
    Temporary replacement for original Feliz.UseElmish written by @MangelMaxime to enable HMR / Fast Refresh
    See: https://github.com/Zaid-Ajaj/Feliz/issues/481 for more info

    This file will be deleted once original Feliz.UseElmish is fixed
*)

open Feliz
open Elmish

module Resumable =

    module Program =

        type Msg<'UserMsg, 'UserModel> =
            | UserMsg of 'UserMsg
            | SetState of 'UserModel

        let toResumable
            (initialState : 'model option)
            (program : Program<'a, 'model, 'msg, 'view>) =

            let map (model, cmd) =
                model
                , Cmd.map UserMsg cmd

            let mapInit userInit args =
                match initialState with
                | Some model ->
                    model, Cmd.none
                | None ->
                    userInit args |> map

            let mapUpdate userUpdate msg model =
                match msg with
                | UserMsg userMsg ->
                    userUpdate userMsg model
                | SetState newModel ->
                    newModel
                    , Cmd.none
                |> map

            let subs userSubscribe model =
                Cmd.batch [
                    userSubscribe model |> Cmd.map UserMsg
                ]

            let mapSetState userSetState model dispatch =
                userSetState model (UserMsg >> dispatch)

            let mapView userView model dispatch =
                userView model (UserMsg >> dispatch)

            program
            |> Program.map mapInit mapUpdate mapView mapSetState subs

type ElmishObservable<'Model, 'Msg>() =
    let mutable hasDisposedOnce = false
    let mutable state: 'Model option = None
    let mutable listener: ('Model -> unit) option = None
    let mutable dispatcher: ('Msg -> unit) option = None

    member _.Value = state
    member _.HasDisposedOnce = hasDisposedOnce

    member _.SetState (model: 'Model) (dispatch: 'Msg -> unit) =
        state <- Some model
        dispatcher <- Some dispatch
        match listener with
        | None -> ()
        | Some listener -> listener model

    member _.Dispatch(msg) =
        match dispatcher with
        | None -> () // Error?
        | Some dispatch -> dispatch msg

    member _.Subscribe(f) =
        match listener with
        | Some _ -> ()
        | None -> listener <- Some f

    /// Disposes state (and dispatcher) but keeps subscription
    member _.DisposeState() =
        match state with
        | Some state ->
            match box state with
            | :? System.IDisposable as disp -> disp.Dispose()
            | _ -> ()
        | _ -> ()
        dispatcher <- None
        state <- None
        hasDisposedOnce <- true

let inline runProgram
    (program: unit -> Program<'Arg, 'Model, 'Msg, unit>)
    (arg: 'Arg)
    (obs: ElmishObservable<'Model, 'Msg>)
    (resumedState: 'Model option)
    () =

    program()
    |> Program.withSetState obs.SetState
    |> Resumable.Program.toResumable resumedState
    |> Program.runWith arg

    match obs.Value with
    | None -> failwith "Elmish program has not initialized"
    | Some v -> v

let inline disposeState (state: obj) =
    match box state with
    | :? System.IDisposable as disp -> disp.Dispose()
    | _ -> ()

type React with
    [<Hook>]
    static member inline useElmish(program: unit -> Program<'Arg, 'Model, 'Msg, unit>, arg: 'Arg, ?dependencies: obj array) =
        // Don't use useMemo here because React doesn't guarantee it won't recreate it again
        let obs, _ = React.useState(fun () -> ElmishObservable<'Model, 'Msg>())

        // Initialize the program without previous state
        let state, setState = React.useState(runProgram program arg obs None)

        // Store initial dependencies
        let deps, setDeps = React.useState(dependencies)

        React.useEffect((fun () ->

            // If the component has been disposed, re-build it
            // use the last known state.
            // This make HMR support works by maintaining the state
            // We need to rebuild a new program, in order to get access
            // to the new dispatch instance
            if obs.HasDisposedOnce then
                // Use only state that is valid for current dependencies
                let state = if deps <> dependencies then None else Some state
                runProgram program arg obs state () |> setState

            // Update for dependencies is the state valid
            dependencies |> setDeps

            React.createDisposable(obs.DisposeState)
        ), defaultArg dependencies [||])

        obs.Subscribe(setState)
        state, obs.Dispatch

    [<Hook>]
    static member inline useElmish(program: unit -> Program<unit, 'Model, 'Msg, unit>, ?dependencies: obj array) =
        React.useElmish(program, (), ?dependencies=dependencies)

    [<Hook>]
    static member inline useElmish(init: 'Arg -> 'Model * Cmd<'Msg>, update: 'Msg -> 'Model -> 'Model * Cmd<'Msg>, arg: 'Arg, ?dependencies: obj array) =
        React.useElmish((fun () -> Program.mkProgram init update (fun _ _ -> ())), arg, ?dependencies=dependencies)

    [<Hook>]
    static member inline useElmish(init: unit -> 'Model * Cmd<'Msg>, update: 'Msg -> 'Model -> 'Model * Cmd<'Msg>, ?dependencies: obj array) =
        React.useElmish((fun () -> Program.mkProgram init update (fun _ _ -> ())), ?dependencies=dependencies)

    [<Hook>]
    static member inline useElmish(init: 'Model * Cmd<'Msg>, update: 'Msg -> 'Model -> 'Model * Cmd<'Msg>, ?dependencies: obj array) =
        React.useElmish((fun () -> Program.mkProgram (fun () -> init) update (fun _ _ -> ())), ?dependencies=dependencies)