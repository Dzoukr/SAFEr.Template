module SAFEr.App.Client.Pages.Home.State

open Domain
open Elmish

let init () = { ChangeMe = "Change me" }, Cmd.none

let update (msg:Msg) (model:Model) : Model * Cmd<Msg> =
    match msg with
    | Loaded -> model, Cmd.none