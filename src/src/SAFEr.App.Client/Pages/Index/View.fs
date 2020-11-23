module SAFEr.App.Client.Pages.Index.View

open Feliz
open Feliz.Bulma
open State
open Feliz.UseElmish

let view = React.functionComponent(fun () ->
    let model, dispatch = React.useElmish(State.init, State.update, [| |])
    Bulma.button.button [
        color.isPrimary
        prop.text model.Message
        prop.onClick (fun _ -> GetMessage |> dispatch)
    ]
)
