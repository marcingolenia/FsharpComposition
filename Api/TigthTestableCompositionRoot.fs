module NotTestableCompositionRoot

  open Stock.StockItem
  open Stock.Application
  open Stock.PostgresDao
  open Settings

  type TightCompositionRoot =
    { QueryStockItemBy: Queries.StockItemById.Query -> Async<Queries.StockItemById.Result option>
      RemoveFromStock: int64 -> int -> Async<Result<unit, StockItemErrors>>
      CreateStockItem: int64 -> string -> int -> Async<unit>
      GenerateId: unit -> int64
    }
    
  let compose settings =
    let createSqlConnection = DapperFSharp.createSqlConnection settings.SqlConnectionString
    let idGenerator = IdGenerator.create settings.IdGeneratorSettings
    let roomIo: StockItemWorkflows.IO = {
      ReadBy = StockItemDao.readBy createSqlConnection
      Update = StockItemDao.update createSqlConnection
      Insert = StockItemDao.insert createSqlConnection
    }
    {
      QueryStockItemBy = RoomQueryDao.readBy createSqlConnection
      RemoveFromStock = StockItemWorkflows.remove roomIo
      CreateStockItem = StockItemWorkflows.create roomIo
      GenerateId = idGenerator
    }
