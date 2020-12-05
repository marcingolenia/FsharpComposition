module NotTestableCompositionRoot

  open Hotel.Room
  open Hotel.Application
  open Hotel.PostgresDao
  open Settings

  type NotTestableCompositionRoot =
    { QueryRoomBy: Queries.RoomById.Query -> Async<Queries.RoomById.Result option>
      BookRoom: int64 -> int -> Async<Result<unit, BookingError>>
      CreateRoom: int64 -> int -> Async<unit>
      GenerateId: unit -> int64
    }
    
  let compose settings =
    let createSqlConnection = DapperFSharp.createSqlConnection settings.SqlConnectionString
    let idGenerator = IdGenerator.create settings.IdGeneratorSettings
    let roomIo: RoomWorkflows.IO = {
      ReadBy = RoomDao.readBy createSqlConnection
      Update = RoomDao.update createSqlConnection
      Create = RoomDao.create createSqlConnection
    }
    {
      QueryRoomBy = RoomQueryDao.readBy createSqlConnection
      BookRoom = RoomWorkflows.bookRoom roomIo
      CreateRoom = RoomWorkflows.createRoom roomIo
      GenerateId = idGenerator
    }
