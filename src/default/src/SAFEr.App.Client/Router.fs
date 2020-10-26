module SAFEr.App.Client.Router

open Browser.Types
open Feliz.Router
open Fable.Core.JsInterop

type Page =
    | Home
    | About

[<RequireQualifiedAccess>]
module Page =
    let defaultPage = Page.Home

    let parseFromUrlSegments = function
        | [ "about" ] -> Page.About
        | [ "" ] -> Page.Home
        | _ -> defaultPage

    let toUrlSegments = function
        | Page.Home -> [ "" ]
        | Page.About -> [ "about" ]


[<RequireQualifiedAccess>]
module Router =
    let goToUrl (e:MouseEvent) =
        e.preventDefault()
        let href : string = !!e.currentTarget?attributes?href?value
        Router.navigatePath href

    let navigatePage (p:Page) = p |> Page.toUrlSegments |> Array.ofList |> Router.navigatePath