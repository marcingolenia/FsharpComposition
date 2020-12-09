namespace Api

module Dtos =
    type RemoveStockItemDto = {
        Id: int64
        Amount: int 
    }
    
    type CreateStockItemDto = {
        Name: string 
        Amount: int
    }

