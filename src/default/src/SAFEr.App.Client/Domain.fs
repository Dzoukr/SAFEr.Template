module SAFEr.App.Client.Domain

open Router

type Model = {
    CurrentPage : Page
}

type Msg =
    | UrlChanged of currentPage:Page