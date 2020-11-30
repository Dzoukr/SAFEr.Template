module SAFEr.App.Client.Pages.Index.View

open Feliz
open Feliz.Bulma
open State
open Feliz.UseElmish

[<ReactComponent>]
let indexView () =
    let model, dispatch = React.useElmish(State.init, State.update, [| |])

    Bulma.button.button [
        color.isPrimary
        prop.text model.Message
        prop.onClick (fun _ -> GetMessage |> dispatch)
    ]