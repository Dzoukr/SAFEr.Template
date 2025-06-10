[<AutoOpen>]
module SAFEr.App.Client.Extensions

open Feliz

type Html
    with
        static member inline classed fn (cn:string) (elm:ReactElement list) =
            fn [
                prop.className cn
                prop.children elm
            ]
        static member inline divClassed (cn:string) (elm:ReactElement list) = Html.classed Html.div cn elm

module Cmd =
    open Elmish

    module OfFunc =
        /// Command to evaluate a simple function
        let func (fn: unit -> unit) : Cmd<'msg> =
            let bind dispatch =
                try fn()
                with x -> ()
            [bind]