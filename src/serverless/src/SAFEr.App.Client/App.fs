module SAFEr.App.Client.App

open Elmish
open Elmish.React

//-:cnd:noEmit
#if DEBUG
open Elmish.Debug
open Elmish.HMR
#endif

//+:cnd:noEmit
Program.mkProgram View.init View.update View.AppView
//-:cnd:noEmit
#if DEBUG
|> Program.withConsoleTrace
#endif
|> Program.withReactSynchronous "safer-app"
#if DEBUG
|> Program.withDebugger
#endif
//+:cnd:noEmit
|> Program.run