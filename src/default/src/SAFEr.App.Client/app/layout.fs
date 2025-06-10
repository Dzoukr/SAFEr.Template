module SAFEr.App.Client.App.Layout

open Feliz
open Fable.Core
open Fable.Core.JsInterop

importSideEffects "./globals.css"

let RootLayout (x:{|children : ReactElement|}) =
    Html.html [
        Html.head [
            Html.title "SAFEr App"
        ]
        Html.body [
            x.children
        ]
    ]

RootLayout |> exportDefault //exportDefault is important for next js