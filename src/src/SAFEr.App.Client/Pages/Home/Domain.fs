module SAFEr.App.Client.Pages.Home.Domain

type Model = {
    Message : string
}

type Msg =
    | GetMessage
    | GotMessage of string