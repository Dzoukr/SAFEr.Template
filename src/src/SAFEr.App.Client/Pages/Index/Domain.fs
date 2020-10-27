module SAFEr.App.Client.Pages.Index.Domain

type Model = {
    Message : string
}

type Msg =
    | GetMessage
    | GotMessage of string