namespace Hotel.Application

open Hotel.Room
open FsToolkit.ErrorHandling

module RoomWorkflows =
    type IO = {
        ReadBy: RoomId -> Async<Room>
        Update: Room -> Async<unit>
        Create: FreeRoom -> Async<unit>
    }
       
    let bookRoom io id guestNumber: Async<Result<unit, BookingError>> =
        asyncResult {
            let! room = io.ReadBy (id |> RoomId)
            let! bookedRoom = bookRoom room guestNumber
            do! io.Update bookedRoom
        }
    
    let createRoom io id capacity: Async<unit> =
        createRoom (id |> RoomId) capacity
        |> io.Create