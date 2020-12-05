namespace Hotel.Application

open Hotels.Room
open FsToolkit.ErrorHandling

module BookHotelWorkflow =
    type IO = {
        ReadBy: RoomId -> Async<Room>
        Save: Room -> Async<unit>
    }
    
//    let bookHotel io id guestNumber: Async<Result<unit, BookingError>> =
//        async {
//            let! room = io.ReadBy (id |> RoomId)
//            let bookingTrial = bookRoom room guestNumber
//            return bookingTrial |> Result.bind (fun bookedRoom -> io.Save bookedRoom |> Async.RunSynchronously |> Ok )
//        }
   
    let bookHotel io id guestNumber:  Async<Result<unit, BookingError>> =
        asyncResult {
            let! room = io.ReadBy (id |> RoomId)
            let! bookedRoom = bookRoom room guestNumber
            do! io.Save bookedRoom
        }
        
