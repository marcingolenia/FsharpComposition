namespace Api.FlexibleCompositionRoot

open Settings
open Stock.Application
open Stock.PostgresDao 

module Trunk = 
    type Trunk =
        {
            GenerateId: unit -> int64
            StockItemWorkflowDependencies: StockItemWorkflows.IO
            QueryStockItemBy: Queries.StockItemById.Query -> Async<Queries.StockItemById.Result option>
        }
        
    let compose (settings: Settings) =
        let createDbConnection = DapperFSharp.createSqlConnection settings.SqlConnectionString
        {
            GenerateId = IdGenerator.create settings.IdGeneratorSettings
            StockItemWorkflowDependencies = Leaves.StockItemWorkflowDependencies.compose createDbConnection
            QueryStockItemBy = StockItemQueryDao.readBy createDbConnection
            // Your next application layer workflow dependencies ...
        }


