module Tests2

open System
open Api
open Dtos
open Microsoft.AspNetCore.Http
open Stock.StockItem
open Xunit
open HttpContext
open FSharp.Control.Tasks.V2
open FsUnit.Xunit
open TestFlexibleCompositionRoot

[<Fact>]
let ``GIVEN id of not existing stock item WHEN QueryStockItemBy THEN none is returned and mapped into 404 not found`` () =
    let httpContext = buildMockHttpContext ()
    // This is trivial example - We could as well build the QueryStockItemBy by ourselves and pass it to the handler.
    // To make it easy to understand more complex cases, let me do it by the "Flexible CompositionRoot" way.
    let root = testTrunk
               |> ``with Query -> StockItemById`` (fun _ -> async { return None })
               |> composeRoot
    let http =
        task {
            let! ctxAfterHandler = HttpHandler.queryStockItemHandler root.QueryStockItemBy (int64 5) next httpContext 
            return ctxAfterHandler
        } |> Async.AwaitTask |> Async.RunSynchronously |> Option.get
    http.Response.StatusCode |> should equal StatusCodes.Status404NotFound

[<Fact>]
let ``GIVEN stock item was passed into request WHEN CreateStockItem THEN new stock item is created and location is returned which can be used to fetch created stock item`` () =
    // Arrange
    let (name, amount) = (Guid.NewGuid().ToString(), Random().Next(1, 15))
    let httpContext = buildMockHttpContext ()
                      |> writeObjectToBody {Name = name; Amount = amount}
    // Again we can compose the function by ourself, but using our root we can test both - the function and the
    // dependencies composition (as in this way the dependencies in our tests are composed in the same way.
    let mutable createdStockItem: StockItem option = None
    let root = testTrunk
               |> ``with StockItem -> Insert`` (fun stockItem -> async { createdStockItem <- Some stockItem; return () })
               |> ``with Query -> StockItemById`` (fun _ ->
                   async {
                       let stockItem = createdStockItem.Value
                       let (StockItemId id) = stockItem.Id
                       return ( Some {
                           Id = id
                           AvailableAmount = stockItem.AvailableAmount
                           Name = stockItem.Name
                       }
                               : Queries.StockItemById.Result option)
                   })
               |> composeRoot
    // Act
    let http =
        task {
            let! ctxAfterHandler = HttpHandler.createStockItemHandler root.CreateStockItem root.GenerateId next httpContext 
            return ctxAfterHandler
        } |> Async.AwaitTask |> Async.RunSynchronously |> Option.get
    // Assert
    http.Response.StatusCode |> should equal StatusCodes.Status201Created
    let createdId = http.Response.Headers.["Location"].ToString().[11..] |> Int64.Parse
    let httpContext4Query = buildMockHttpContext ()
    let httpAfterQuery =
        task {
            let! ctxAfterQuery = HttpHandler.queryStockItemHandler root.QueryStockItemBy createdId next httpContext4Query
            return ctxAfterQuery
        } |> Async.AwaitTask |> Async.RunSynchronously |> Option.get
    let createdStockItem = httpAfterQuery.Response |> deserializeResponse<Queries.StockItemById.Result>
    createdStockItem.Id |> should equal createdId
    createdStockItem.Name |> should equal name
    createdStockItem.AvailableAmount |> should equal amount
    
    // Now You! Write Test for Update in both approaches.