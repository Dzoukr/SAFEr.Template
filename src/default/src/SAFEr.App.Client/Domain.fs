module SAFEr.App.Client.Domain

type Page =
    | Home

type Model = {
    CurrentPage : Page
}

type Msg =
    | UrlChanged of currentPage:Page