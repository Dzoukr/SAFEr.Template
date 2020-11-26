module SAFEr.App.Client.Router

open Browser.Types
open Feliz.Router
open Fable.Core.JsInterop

type Page =
    | Index
    | About

[<RequireQualifiedAccess>]
module Page =
    let defaultPage = Page.Index

    let parseFromUrlSegments = function
        | [ "about" ] -> Page.About
        | [ ] -> Page.Index
        | _ -> defaultPage

    let noQueryString segments : string list * (string * string) list = segments, []

    let toUrlSegments = function
        | Page.Index -> [ ] |> noQueryString
        | Page.About -> [ "about" ] |> noQueryString


[<RequireQualifiedAccess>]
module Router =
    let goToUrl (e:MouseEvent) =
        e.preventDefault()
        let href : string = !!e.currentTarget?attributes?href?value
        Router.navigatePath href

    let navigatePath (segments, queryString:(string * string) list) =
        match segments with
        | [] -> Router.navigatePath("", queryString)
        | segs ->
            let last = segs |> List.rev |> List.head
            let rest = segs |> List.rev |> List.tail |> List.rev
            Router.nav (rest @ [ last + Router.encodeQueryString queryString ]) HistoryMode.PushState RouteMode.Path

    let formatPath(segments, queryString: (string * string) list) : string =
        match segments with
        | [] -> Router.formatPath("", queryString)
        | segs ->
            let last = segs |> List.rev |> List.head
            let rest = segs |> List.rev |> List.tail |> List.rev
            Router.encodeParts (rest @ [ last + Router.encodeQueryString queryString ]) RouteMode.Path

    let navigatePage (p:Page) = p |> Page.toUrlSegments |> navigatePath