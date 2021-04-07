module SAFEr.App.Client.Pages.Index

open Feliz
open Feliz.Bulma
open Feliz.UseDeferred
open SAFEr.App.Client

[<ReactComponent>]
let IndexView () =
    let callReq,setCallReq = React.useState(Deferred.HasNotStartedYet)
    let call = React.useDeferredCallback((fun _ -> Server.service.GetMessage()), setCallReq)
    let title =
        match callReq with
        | Deferred.HasNotStartedYet -> "Click me!"
        | Deferred.InProgress -> "...loading"
        | Deferred.Resolved m -> m
        | Deferred.Failed err -> err.Message

    Bulma.button.button [
        color.isPrimary
        prop.text title
        prop.onClick call
    ]