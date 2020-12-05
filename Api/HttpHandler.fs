namespace Api

module HttpHandler =
    open Giraffe
    open Microsoft.AspNetCore.Http
    open NotTestableCompositionRoot
    open FSharp.Control.Tasks.V2.ContextInsensitive

    let queryRoomHandler queryRoomBy (id: int64): HttpHandler =
        fun (next: HttpFunc) (ctx: HttpContext) ->
            task {
                let! room = queryRoomBy (id |> Queries.RoomById.Query)
                return! json room next ctx
            }
            
    let bookRoomHandler bookRoom (id: int64) (guestsNumber: int): HttpHandler =
        fun (next: HttpFunc) (ctx: HttpContext) ->
            task {
                do! bookRoom id guestsNumber
                return! Successful.OK (text "OK") next ctx
            }

    let createRoomHandler createRoom (id: int64) (capacity: int): HttpHandler =
        fun (next: HttpFunc) (ctx: HttpContext) ->
            task {
                do! createRoom id capacity
                ctx.SetHttpHeader "Location" (sprintf "/Room/%d")
                return! Successful.created (text "Created") next ctx
            }

    let router (compositionRoot: NotTestableCompositionRoot): HttpFunc -> HttpContext -> HttpFuncResult =
        choose [ GET >=> route "/" >=> htmlView Views.index
                 GET >=> routef "/Room/%d" (queryRoomHandler compositionRoot.QueryRoomBy)
                 PATCH >=> routef "/Room/%d%i" (bookRoomHandler compositionRoot.BookRoom) // dto
                 POST >=> routef "/Room/%i" (createRoomHandler compositionRoot.CreateRoom (compositionRoot.GenerateId()))
                 setStatusCode 404 >=> text "Not Found" ]
