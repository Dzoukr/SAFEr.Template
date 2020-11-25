module SAFEr.App.Client.State

open Elmish
open Router
open Feliz.Router

type Model = {
    CurrentPage : Page
}

type Msg =
    | UrlChanged of currentPage:Page

let init () =
    let nextPage = (Router.currentPath() |> Page.parseFromUrlSegments)
    { CurrentPage = nextPage }, Cmd.ofSub (fun _ -> Router.navigatePage nextPage)

let update (msg:Msg) (model:Model): Model * Cmd<Msg> =
    match msg with
    | UrlChanged page -> { model with CurrentPage = page }, Cmd.none
