module TestFlexibleCompositionRoot

open System
open Api.FlexibleCompositionRoot
open FlexibleCompositionRoot
open Settings
let testSettings: Settings =
    // We can test with database but we don't have to.
    { SqlConnectionString = "Host=localhost;User Id=postgres;Password=Secret!Passw0rd;Database=stock;Port=5432"
      IdGeneratorSettings =
          { GeneratorId = 555
            Epoch = DateTimeOffset.Parse "2020-10-01 12:30:00"
            TimestampBits = byte 41
            GeneratorIdBits = byte 10
            SequenceBits = byte 12 } }

let composeRoot tree = compose tree
let testTrunk = Trunk.compose testSettings

let ``with StockItem -> ReadBy`` substitute (trunk: Trunk.Trunk) =
  { trunk with StockItemWorkflowDependencies = { trunk.StockItemWorkflowDependencies with ReadBy = substitute } }
  
let ``with StockItem -> Update`` substitute (trunk: Trunk.Trunk) =
  { trunk with StockItemWorkflowDependencies = { trunk.StockItemWorkflowDependencies with Update = substitute } }
  
let ``with StockItem -> Insert`` substitute (trunk: Trunk.Trunk) =
  { trunk with StockItemWorkflowDependencies = { trunk.StockItemWorkflowDependencies with Insert = substitute } }

let ``with Query -> StockItemById`` substitute (trunk: Trunk.Trunk) =
  { trunk with QueryStockItemBy = substitute }