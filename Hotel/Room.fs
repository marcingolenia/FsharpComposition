namespace Hotels

module Room =
    
    type BookingError = | RoomOverbooked of string | RoomAlreadyBooked 
    
    type RoomId = RoomId of int
   
    type FreeRoom = {
        Id: RoomId
        Capacity: int
    }
    
    type Room =
        | FreeRoom of FreeRoom
        | BookedRoom of RoomId
        
    let bookRoom (room: Room) guestsNumber: Result<Room, BookingError>  =
        match room with
            | FreeRoom room when room.Capacity < guestsNumber -> RoomOverbooked "" |> Error
            | FreeRoom room -> room.Id |> BookedRoom |> Ok
            | BookedRoom _ -> RoomAlreadyBooked |> Error