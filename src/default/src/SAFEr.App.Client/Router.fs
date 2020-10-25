module SAFEr.App.Client.Router

open Browser.Types
open Domain
open Feliz.Router
open Fable.Core.JsInterop

[<RequireQualifiedAccess>]
module Page =
    let defaultPage = Page.Home

    let parseFromUrlSegments = function
        | _ -> defaultPage

    let toUrlSegments = function
        | Page.Home -> [ "home" ]


[<RequireQualifiedAccess>]
module Router =
    let goToUrl (e:MouseEvent) =
        e.preventDefault()
        let href : string = !!e.currentTarget?attributes?href?value
        Router.navigatePath href

    let navigatePage (p:Page) = p |> Page.toUrlSegments |> Array.ofList |> Router.navigatePath