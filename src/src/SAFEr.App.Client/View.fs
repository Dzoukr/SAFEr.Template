module SAFEr.App.Client.View

open Domain
open Feliz
open Router
open SharedView

let view (model:Model) (dispatch:Msg -> unit) =
    let navigation =
        Html.div [
            Html.aRouted "Home" Page.Index
            Html.span " | "
            Html.aRouted "About" Page.About
        ]
    let render =
        match model.CurrentPage with
        | Page.Index -> Pages.Index.View.view ()
        | Page.About -> Html.text "SAFEr Template"
    React.router [
        router.pathMode
        router.onUrlChanged (Page.parseFromUrlSegments >> UrlChanged >> dispatch)
        router.children [ navigation; render ]
    ]