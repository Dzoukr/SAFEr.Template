module SAFEr.App.Client.View

open Domain
open Feliz
open Feliz.Bulma
open Router
open Feliz.Router

let view (model:Model) (dispatch:Msg -> unit) =
    let render =
        match model.CurrentPage with
        | Home -> Pages.Home.View.view ()
    React.router [
        router.pathMode
        router.onUrlChanged (Page.parseFromUrlSegments >> UrlChanged >> dispatch)
        router.children render
    ]