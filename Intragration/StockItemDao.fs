module Tests

open System
open Stock
open Xunit
open Stock.PostgresDao
open StockItem
open FsToolkit.ErrorHandling
open FsUnit.Xunit

[<Fact>]
let ``GIVEN stock item WHEN inserted THEN after read it fully restored`` () =
    // Arrange
    let expectedStockItem: StockItem =
        { Id = Toolbox.generateId () |> StockItemId
          AvailableAmount = 4
          Name = Guid.NewGuid().ToString() }
    asyncResult {
        // Act
        do! StockItemDao.insert Toolbox.createSqlConnection expectedStockItem
        // Assert
        let! actualStockItem = StockItemDao.readBy Toolbox.createSqlConnection expectedStockItem.Id
        actualStockItem |> should equal expectedStockItem
    } |> Async.RunSynchronously

[<Fact>]
let ``GIVEN stock item WHEN updated THEN after read it has updated values`` () =
    // Arrange
    let initialStockItem: StockItem =
        { Id = Toolbox.generateId () |> StockItemId
          AvailableAmount = 4
          Name = Guid.NewGuid().ToString() }
    let modifiedStockItem = { initialStockItem with AvailableAmount = 2; Name = Guid.NewGuid.ToString() }
    asyncResult {
        do! StockItemDao.insert Toolbox.createSqlConnection initialStockItem
        // Act
        do! StockItemDao.update Toolbox.createSqlConnection modifiedStockItem
        // Assert
        let! actualStockItem = StockItemDao.readBy Toolbox.createSqlConnection modifiedStockItem.Id
        actualStockItem |> should not' (equal initialStockItem)
        actualStockItem |> should equal modifiedStockItem
    } |> Async.RunSynchronously