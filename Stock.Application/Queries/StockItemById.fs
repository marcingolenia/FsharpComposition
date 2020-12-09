module Queries.StockItemById

type Query = bigint

type Result = {
    Id: int64
    Name: string
    AvailableAmount: int
}
