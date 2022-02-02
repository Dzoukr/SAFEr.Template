module SAFEr.App.Shared.API

type Service = {
    GetMessage : bool -> Async<string>
}
with
    static member RouteBuilder _ m = sprintf "/api/service/%s" m