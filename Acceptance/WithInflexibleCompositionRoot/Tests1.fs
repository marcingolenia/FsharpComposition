module Tests1

open System
open Api
open Dtos
open Microsoft.AspNetCore.Http
open Xunit
open HttpContext
open FSharp.Control.Tasks.V2
open FsUnit.Xunit
open TestInflexibleCompositionRoot

[<Fact>]
let ``GIVEN id of not existing stock item WHEN QueryStockItemBy THEN none is returned and mapped into 404 not found`` () =
    let httpContext = buildMockHttpContext ()
    let http =
        task {
            let! ctxAfterHandler = HttpHandler.queryStockItemHandler testRoot.QueryStockItemBy (int64 5) next httpContext 
            return ctxAfterHandler
        } |> Async.AwaitTask |> Async.RunSynchronously |> Option.get
    http.Response.StatusCode |> should equal StatusCodes.Status404NotFound

[<Fact>]
let ``GIVEN stock item was passed into request WHEN CreateStockItem THEN new stockitem is created and location is returned which can be used to fetch created stockitem`` () =
    // Arrange
    let (name, amount) = (Guid.NewGuid().ToString(), Random().Next(1, 15))
    let httpContext = buildMockHttpContext ()
                      |> writeObjectToBody {Name = name; Amount = amount}
    // Act
    let http =
        task {
            let! ctxAfterHandler = HttpHandler.createStockItemHandler testRoot.CreateStockItem testRoot.GenerateId next httpContext 
            return ctxAfterHandler
        } |> Async.AwaitTask |> Async.RunSynchronously |> Option.get
    // Assert
    http.Response.StatusCode |> should equal StatusCodes.Status201Created
    let createdId = http.Response.Headers.["Location"].ToString().[11..] |> Int64.Parse
    let httpContext4Query = buildMockHttpContext ()
    let httpAfterQuery =
        task {
            let! ctxAfterQuery = HttpHandler.queryStockItemHandler testRoot.QueryStockItemBy createdId next httpContext4Query
            return ctxAfterQuery
        } |> Async.AwaitTask |> Async.RunSynchronously |> Option.get
    let createdStockItem = httpAfterQuery.Response |> deserializeResponse<Queries.StockItemById.Result>
    createdStockItem.Id |> should equal createdId
    createdStockItem.Name |> should equal name
    createdStockItem.AvailableAmount |> should equal amount
    