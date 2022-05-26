module SAFEr.App.Client.Pages.Index

open Feliz
open Feliz.DaisyUI
open Elmish
open Feliz.UseElmish
open SAFEr.App.Client.Server

type private State = {
    Message : string
}

type private Msg =
    | AskForMessage of bool
    | MessageReceived of ServerResult<string>

let private init () = { Message = "Click on one of the buttons!" }, Cmd.none

let private update (msg:Msg) (model:State) : State * Cmd<Msg> =
    match msg with
    | AskForMessage success -> model, Cmd.OfAsync.eitherAsResult (fun _ -> service.GetMessage success) MessageReceived
    | MessageReceived (Ok msg) -> { model with Message = $"Got success response: {msg}" }, Cmd.none
    | MessageReceived (Error error) -> { model with Message = $"Got server error: {error}" }, Cmd.none

[<ReactComponent>]
let IndexView () =
    let state, dispatch = React.useElmish(init, update, [| |])

    React.fragment [
        Html.div state.Message
        Daisy.buttonGroup [
            Daisy.button.button [
                button.primary
                prop.text "Click me for success"
                prop.onClick (fun _ -> true |> AskForMessage |> dispatch)
            ]
            Daisy.button.button [
                button.secondary
                prop.text "Click me for error"
                prop.onClick (fun _ -> false |> AskForMessage |> dispatch)
            ]
        ]
    ]