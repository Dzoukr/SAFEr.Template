module SAFEr.App.Client.SharedView

open Feliz
open Router

type prop
    with
        static member inline href (p:Page) = prop.href (p |> Page.toUrlSegments |> Router.formatPath)
        static member inline onClick (p:Page) = prop.onClick (Router.goToUrl)

type Html
    with
        static member inline a (text:string, p:Page) =
            Html.a [
                prop.href p
                prop.onClick p
                prop.text text
            ]

