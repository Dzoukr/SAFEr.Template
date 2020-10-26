module SAFEr.App.Client.SharedView

open Feliz
open Router

module Html =
    module Props =
        let routed (p:Page) =
            [
                prop.href (p |> Page.toUrlSegments |> Router.formatPath)
                prop.onClick (Router.goToUrl)
            ]

    let aRouted (text:string) (p:Page) =
        Html.a [
            yield! Props.routed p
            prop.text text
        ]