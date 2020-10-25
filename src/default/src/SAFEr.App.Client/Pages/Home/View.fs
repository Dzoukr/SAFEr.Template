module SAFEr.App.Client.Pages.Home.View

open Feliz
open Feliz.Bulma
open Domain
open Feliz.UseElmish

let view = React.functionComponent(fun () ->
    let model, dispatch = React.useElmish(State.init, State.update, [| |])
    Html.div [ prop.text model.ChangeMe ]
)