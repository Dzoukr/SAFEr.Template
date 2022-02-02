module SAFEr.App.Shared.API

type Service = {
    GetMessage : bool -> Async<string>
}
with
    static member RouteBuilder s m = sprintf "/api/%s/%s" s m