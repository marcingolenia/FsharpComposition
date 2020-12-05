namespace Hotel.PostgresDao

open Hotel.Room
open DapperFSharp

module RoomDao =

    let readBy createConnection (roomId: RoomId) =
        async {
            use! connection = createConnection ()
            return! connection |> sqlSingle<Room> "" roomId
        }

    let create createConnection room =
        async {
            use! connection = createConnection ()
            do! connection |> sqlExecute "" room
        }

    let update createConnection room =
        async {
            use! connection = createConnection ()
            do! connection |> sqlExecute "" room
        }
