namespace Stock

module StockItem =
    
    type StockItemErrors = | OutOfStock | CannotReadStockItem
    type StockItemId = StockItemId of int64
    
    type StockItem = {
        Id: StockItemId
        Name: string
        AvailableAmount: int
    }
        
    let create id name amount =
       { Id = id; Name = name; AvailableAmount = amount }
       
    let (|GreaterThan|_|) k value = if value <= k then Some() else None
        
    let remove (stockItem: StockItem) amount: Result<StockItem, StockItemErrors> =
        match amount with
        | GreaterThan stockItem.AvailableAmount ->
            { stockItem with AvailableAmount = stockItem.AvailableAmount - amount } |> Ok
        | _ -> OutOfStock |> Error