module SAFEr.App.Client.View

open Feliz
open Router
open SharedView

[<ReactComponent>]
let AppView () =
    let page,setPage = React.useState(Router.currentPath() |> Page.parseFromUrlSegments)

    // routing for full refreshed page (to fix wrong urls)
    React.useEffectOnce (fun _ -> Router.navigatePage page)

    let navigation =
        Html.div [
            Html.a("Home", Page.Index)
            Html.span " | "
            Html.a("About", Page.About)
        ]
    let render =
        match page with
        | Page.Index -> Pages.Index.IndexView ()
        | Page.About -> Html.text "SAFEr Template"
    React.router [
        router.pathMode
        router.onUrlChanged (Page.parseFromUrlSegments >> setPage)
        router.children [ navigation; render ]
    ]