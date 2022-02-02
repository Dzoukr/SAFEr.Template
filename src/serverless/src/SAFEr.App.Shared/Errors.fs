module SAFEr.App.Shared.Errors

type ServerError =
    | Exception of string
    | Authentication of string

exception ServerException of ServerError

module ServerError =
    let failwith (er:ServerError) = raise (ServerException er)

    let ofResult<'a> (v:Result<'a,ServerError>) =
        match v with
        | Ok v -> v
        | Error e -> e |> failwith
