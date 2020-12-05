namespace Hotel.PostgresDao

open DapperFSharp

module RoomQueryDao =

    let readBy createConnection (roomId: Queries.RoomById.Query) =
        async {
            use! connection = createConnection ()
            return! connection |> sqlSingleOrNone<Queries.RoomById.Result> "" roomId
        }
