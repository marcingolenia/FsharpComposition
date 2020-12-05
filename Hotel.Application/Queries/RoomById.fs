module Queries.RoomById

type Query = bigint

type Result = {
    Id: bigint
    Capacity: int
    IsBooked: bool
}
