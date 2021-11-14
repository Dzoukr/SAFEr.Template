module SAFEr.App.Client.View

open Feliz
open Router
open Elmish
open SharedView

type Msg =
    | UrlChanged of Page

type State = {
    Page : Page
}

let init () =
    let nextPage = Router.currentPath() |> Page.parseFromUrlSegments
    { Page = nextPage }, Cmd.navigatePage nextPage

let update (msg:Msg) (state:State) : State * Cmd<Msg> =
    match msg with
    | UrlChanged page -> { state with Page = page }, Cmd.none

[<ReactComponent>]
let AppView (state:State) (dispatch:Msg -> unit) =
    let navigation =
        Html.div [
            Html.a("Home", Page.Index)
            Html.span " | "
            Html.a("About", Page.About)
        ]
    let render =
        match state.Page with
        | Page.Index -> Pages.Index.IndexView ()
        | Page.About -> Html.text "SAFEr Template"
    React.router [
        router.pathMode
        router.onUrlChanged (Page.parseFromUrlSegments >> UrlChanged >> dispatch)
        router.children [ navigation; render ]
    ]