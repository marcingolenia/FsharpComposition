module FlexibleCompositionRoot
  open Api.FlexibleCompositionRoot
  open Stock.Application
  open Stock.StockItem

  type FlexibleCompositionRoot =
    { QueryStockItemBy: Queries.StockItemById.Query -> Async<Queries.StockItemById.Result option>
      RemoveFromStock: int64 -> int -> Async<Result<unit, StockItemErrors>>
      CreateStockItem: int64 -> string -> int -> Async<unit>
      GenerateId: unit -> int64
    }
    
  let compose (trunk: Trunk.Trunk) =
    {
      QueryStockItemBy = trunk.QueryStockItemBy
      RemoveFromStock = StockItemWorkflows.remove trunk.StockItemWorkflowDependencies
      CreateStockItem = StockItemWorkflows.create trunk.StockItemWorkflowDependencies
      GenerateId = trunk.GenerateId
    }