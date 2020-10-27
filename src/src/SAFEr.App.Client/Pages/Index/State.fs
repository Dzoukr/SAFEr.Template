module SAFEr.App.Client.Pages.Index.State

open Domain
open Elmish
open SAFEr.App.Client

let init () = { Message = "Click me!" }, Cmd.none

let update (msg:Msg) (model:Model) : Model * Cmd<Msg> =
    match msg with
    | GetMessage -> model, Cmd.OfAsync.perform Server.service.GetMessage () GotMessage
    | GotMessage msg -> { model with Message = msg }, Cmd.none