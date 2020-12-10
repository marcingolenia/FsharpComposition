namespace Api.FlexibleCompositionRoot.Leaves

open System.Data
open Stock.Application
open Stock.PostgresDao

module StockItemWorkflowDependencies =
    let compose (createDbConnection: unit -> Async<IDbConnection>) : StockItemWorkflows.IO =
        {
            ReadBy = StockItemDao.readBy createDbConnection
            Update = StockItemDao.update createDbConnection
            Insert = StockItemDao.insert createDbConnection
        }

