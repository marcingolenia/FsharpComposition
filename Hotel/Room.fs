namespace Hotel

module Room =
    
    type BookingError = | RoomOverbooked of string | RoomAlreadyBooked
    
    type RoomId = RoomId of int64
   
    type FreeRoom = {
        Id: RoomId
        Capacity: int
    }
    
    type BookedRoom = {
        Id: RoomId
        Capacity: int
        Guests: int
    }
    
    type Room =
        | FreeRoom of FreeRoom
        | BookedRoom of BookedRoom
        
    let createRoom id capacity =
       { Id = id; Capacity = capacity }
        
    let bookRoom (room: Room) guestsNumber: Result<Room, BookingError>  =
        match room with
            | FreeRoom room when room.Capacity < guestsNumber
                 -> RoomOverbooked "Room capacity is {room.capacity} so cannot accommodate {guestsNumber} people." |> Error
            | FreeRoom room -> BookedRoom { Id = room.Id; Capacity = room.Capacity; Guests = guestsNumber } |> Ok
            | BookedRoom _ -> RoomAlreadyBooked |> Error