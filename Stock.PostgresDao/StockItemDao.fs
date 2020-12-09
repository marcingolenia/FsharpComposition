namespace Stock.PostgresDao

open Stock.StockItem
open DapperFSharp
open Thoth.Json.Net

module StockItemDao =
    let readBy createConnection (id: StockItemId) =
        async {
            use! connection = createConnection ()
            let (StockItemId id) = id
            let! stockItemJson = connection |> sqlSingle<string> "SELECT data FROM stockitems WHERE id = @id" {|id = id|}
            let stockItem = Decode.Auto.fromString<StockItem>(stockItemJson, CaseStrategy.PascalCase, Extra.empty |> Extra.withInt64)
            return stockItem
        }

    let insert createConnection (stockItem: StockItem) =
        async {
            use! connection = createConnection ()
            let (StockItemId id) = stockItem.Id
            let json = Encode.Auto.toString(4, stockItem, CaseStrategy.PascalCase, Extra.empty |> Extra.withInt64)
            do! connection |> sqlExecute "
            INSERT INTO stockitems
            (id, data)
            VALUES(@Id, @Data::jsonb);" {| Id = id; Data = json |}
        }

    let update createConnection stockItem =
        async {
            use! connection = createConnection ()
            let (StockItemId id) = stockItem.Id
            let json = Encode.Auto.toString(4, stockItem, CaseStrategy.PascalCase, Extra.empty |> Extra.withInt64)
            do! connection |> sqlExecute "
            UPDATE stockitems
            SET data = @Data::jsonb
            WHERE id = @Id" {| Id = id; Data = json |}
        }
