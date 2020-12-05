namespace Api
module HttpHandler =

    open Giraffe
    open Microsoft.AspNetCore.Http

    let router: HttpFunc -> HttpContext -> HttpFuncResult =
        choose [
            GET >=> route "/" >=> htmlView Views.index
            setStatusCode 404 >=> text "Not Found"
            ]
