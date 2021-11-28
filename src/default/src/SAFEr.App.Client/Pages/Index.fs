module SAFEr.App.Client.Pages.Index

open Feliz
open Feliz.DaisyUI
open Elmish
open Feliz.UseElmish
open SAFEr.App.Client

type State = {
    Message : string
}

type Msg =
    | AskForMessage
    | MessageReceived of string

let init () = { Message = "Click me!" }, Cmd.none

let update (msg:Msg) (model:State) : State * Cmd<Msg> =
    match msg with
    | AskForMessage -> model, Cmd.OfAsync.perform Server.service.GetMessage () MessageReceived
    | MessageReceived msg -> { model with Message = msg }, Cmd.none

[<ReactComponent>]
let IndexView () =
    let state, dispatch = React.useElmish(init, update, [| |])

    Daisy.button.button [
        button.primary
        prop.text state.Message
        prop.onClick (fun _ -> AskForMessage |> dispatch)
    ]