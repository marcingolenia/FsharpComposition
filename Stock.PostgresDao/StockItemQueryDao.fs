namespace Stock.PostgresDao

open DapperFSharp

module RoomQueryDao =

    let readBy createConnection (id: Queries.StockItemById.Query) =
        async {
            use! connection = createConnection ()
            return! connection |> sqlSingleOrNone<Queries.StockItemById.Result> "
            SELECT Id,
            data ->> 'Name' AS Name,
            (data ->> 'AvailableAmount')::int AS AvailableAmount
            FROM stockitems 
            WHERE Id = @Id
            " {|Id = id |> int64|}
        }