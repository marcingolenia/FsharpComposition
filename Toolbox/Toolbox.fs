module Toolbox

    open System
    open System.Data
    open IdGen
    open Npgsql
    
    let createSqlConnection: unit -> Async<IDbConnection> =
        fun () -> async {
          let connection = new NpgsqlConnection("Host=localhost;User Id=postgres;Password=Secret!Passw0rd;Database=stock;Port=5432")
          if connection.State <> ConnectionState.Open
          then do! connection.OpenAsync() |> Async.AwaitTask
          return connection :> IDbConnection
        }
    
    let generateId =
        let structure = IdStructure(byte 41, byte 10, byte 12)
        let options = IdGeneratorOptions(structure, DefaultTimeSource(DateTimeOffset.Parse "2020-10-01 12:30:00"))
        let generator = IdGenerator(666, options)
        fun() -> generator.CreateId()