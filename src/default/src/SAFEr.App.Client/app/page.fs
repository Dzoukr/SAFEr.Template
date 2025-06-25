module Safer.App.App.Page

open Feliz
open Feliz.DaisyUI
open Elmish
open SAFEr.App.Client.Server
open Feliz.UseElmish
open Fable.Core.JsInterop
open Feliz.DaisyUI



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



let Page () =
    Html.div [
        SAFEr.App.Client.components.test.Test()
        Html.div "HI PAGE"
    ]


let generateMetadata () =
    {| title = "TEST"  |}

Page |> exportDefault //exportDefault is important for next js