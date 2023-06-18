namespace Api

open Api.Dtos
open FlexibleCompositionRoot
open Stock.StockItem
open Microsoft.AspNetCore.Http
open FSharp.Control
open Giraffe

module HttpHandler =
    let queryStockItemHandler queryStockItemBy (id: int64): HttpHandler =
        fun (next: HttpFunc) (ctx: HttpContext) ->
            task {
                let! stockItem = queryStockItemBy (id |> Queries.StockItemById.Query) |> Async.StartAsTask
                return! match stockItem with
                        | Some stockItem -> json stockItem next ctx
                        | None -> RequestErrors.notFound (text "Not Found") next ctx
            }
            
    let removeFromStockItem (removeStockItem: int64 -> int -> Async<Result<unit, StockItemErrors>>): HttpHandler =
        fun (next: HttpFunc) (ctx: HttpContext) ->
            task {
                let! stockItemDto = ctx.BindJsonAsync<RemoveStockItemDto>()
                let! bookingResult = removeStockItem stockItemDto.Id stockItemDto.Amount
                let response = match bookingResult with
                               | Ok _ -> Successful.OK (text "OK") next ctx
                               | Error message -> RequestErrors.badRequest (json message) next ctx
                return! response
            }

    let createStockItemHandler createStockItem (createId: unit -> int64): HttpHandler =
        fun (next: HttpFunc) (ctx: HttpContext) ->
            let id = createId()
            task {
                let! stockItemDto = ctx.BindJsonAsync<CreateStockItemDto>()
                do! createStockItem id stockItemDto.Name stockItemDto.Amount |> Async.StartAsTask
                ctx.SetHttpHeader("Location", (sprintf "/stockitem/%d" id))
                return! Successful.created (text "Created") next ctx
            }

    let router (compositionRoot: FlexibleCompositionRoot): HttpFunc -> HttpContext -> HttpFuncResult =
        choose [ GET >=> route "/" >=> htmlView Views.index
                 GET >=> routef "/stockitem/%d" (queryStockItemHandler compositionRoot.QueryStockItemBy)
                 PATCH >=> route "/stockitem/" >=> (removeFromStockItem compositionRoot.RemoveFromStock) 
                 POST >=> route "/stockitem/" >=> (createStockItemHandler compositionRoot.CreateStockItem compositionRoot.GenerateId)
                 setStatusCode 404 >=> text "Not Found" ]
