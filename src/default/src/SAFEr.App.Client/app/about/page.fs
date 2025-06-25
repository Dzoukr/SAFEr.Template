module SAFEr.App.Client.App.About.Page

open Feliz
open Feliz.DaisyUI
open Elmish
open SAFEr.App.Client.Server
open Feliz.UseElmish
open Fable.Core.JsInterop

let Page () =
    React.fragment [
        Daisy.alert [
            alert.warning

            prop.text "This is a about page!"
        ]
    ]

Page |> exportDefault //exportDefault is important for next js