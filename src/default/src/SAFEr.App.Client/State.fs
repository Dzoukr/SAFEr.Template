module SAFEr.App.Client.State

open Elmish
open Domain
open Router
open Feliz.Router

let init () =
    let nextPage = (Router.currentPath() |> Page.parseFromUrlSegments)
    {
        CurrentPage = Page.defaultPage
    }, (nextPage |> UrlChanged |> Cmd.ofMsg)

let update (msg:Msg) (model:Model): Model * Cmd<Msg> =
    match msg with
    | UrlChanged page -> { model with CurrentPage = page }, Cmd.none
