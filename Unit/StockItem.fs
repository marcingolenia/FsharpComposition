module Tests

open Xunit
open Stock.StockItem
open FsUnit.Xunit

[<Fact>]
let ``GIVEN amount to remove is greater than available stock item amount WHEN remove THEN OutOfStock error is returned`` () =
    // Arrange
    let expectedError: Result<StockItem, StockItemErrors> = OutOfStock |> Result.Error

    let stockItem =
        { Id = 200 |> int64 |> StockItemId
          Name = "Whatever"
          AvailableAmount = 1 }
    // Act
    let result = remove stockItem 10
    // Assert
    result |> should equal expectedError

[<Theory>]
[<InlineData(1)>]
[<InlineData(0)>]
[<InlineData(4)>]
let ``GIVEN amount to remove is equal or less than available stock item amount WHEN remove THEN StockItem available amount`` (amountToRemove) =
    // Arrange
    let stockItemUnderTest =
        { Id = 200 |> int64 |> StockItemId
          Name = "Whatever"
          AvailableAmount = 4 }
    let expectedStockItem: Result<StockItem, StockItemErrors> =
        { Id = stockItemUnderTest.Id
          Name = stockItemUnderTest.Name
          AvailableAmount = stockItemUnderTest.AvailableAmount - amountToRemove }
        |> Result.Ok
    // Act
    let result = remove stockItemUnderTest amountToRemove
    // Assert
    result |> should equal expectedStockItem
