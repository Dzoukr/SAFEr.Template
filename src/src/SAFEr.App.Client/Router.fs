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

    let private trySeparateLast xs =
        match xs |> List.rev with
        | [] -> None
        | [ single ] -> Some([], single)
        | list -> Some (list |> List.tail |> List.rev, list.Head)

    let navigatePath (segments, queryString:(string * string) list) =
        segments
        |> trySeparateLast
        |> Option.map (fun (r, l) ->
            Router.nav (r @ [ l + Router.encodeQueryString queryString ]) HistoryMode.PushState RouteMode.Path
        )
        |> Option.defaultWith (fun _ -> Router.navigatePath("", queryString))

    let formatPath(segments, queryString: (string * string) list) : string =
        segments
        |> trySeparateLast
        |> Option.map (fun (r, l) ->
            Router.encodeParts (r @ [ l + Router.encodeQueryString queryString ]) RouteMode.Path
        )
        |> Option.defaultWith (fun _ -> Router.formatPath("", queryString))

    let navigatePage (p:Page) = p |> Page.toUrlSegments |> navigatePath