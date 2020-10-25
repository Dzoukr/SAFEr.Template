module SAFEr.App.Shared.Communication

type Service = {
    GetSomething : unit -> Async<string>
}
with
    static member RouteBuilder _ m = sprintf "/api/service/%s" m