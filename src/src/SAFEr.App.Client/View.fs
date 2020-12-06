module SAFEr.App.Client.View

open State
open Feliz
open Feliz.UseElmish
open Router
open SharedView

[<ReactComponent>]
let AppView () =
    let model, dispatch = React.useElmish(State.init, State.update, [| |])

    let navigation =
        Html.div [
            Html.a("Home", Page.Index)
            Html.span " | "
            Html.a("About", Page.About)
        ]
    let render =
        match model.CurrentPage with
        | Page.Index -> Pages.Index.View.IndexView ()
        | Page.About -> Html.text "SAFEr Template"
    React.router [
        router.pathMode
        router.onUrlChanged (Page.parseFromUrlSegments >> UrlChanged >> dispatch)
        router.children [ navigation; render ]
    ]