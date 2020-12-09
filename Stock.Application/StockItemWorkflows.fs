namespace Stock.Application

open Stock.StockItem
open FsToolkit.ErrorHandling

module StockItemWorkflows =
    type IO = {
        ReadBy: StockItemId -> Async<Result<StockItem, string>>
        Update: StockItem -> Async<unit>
        Insert: StockItem -> Async<unit>
    }
       
    let remove io id amount: Async<Result<unit, StockItemErrors>> =
        asyncResult {
            let! stockItem = io.ReadBy (id |> StockItemId) |> AsyncResult.mapError(fun _ -> CannotReadStockItem)
            let! stockItem = remove stockItem amount
            do! io.Update stockItem
        }
    
    let create io id name capacity: Async<unit> =
        create (id |> StockItemId) name capacity
        |> io.Insert