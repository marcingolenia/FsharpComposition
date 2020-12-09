module TestCompositionRoot

open System
open NotTestableCompositionRoot
open Settings
let testSettings: Settings =
    // We are forced to test against database
    { SqlConnectionString = "Host=localhost;User Id=postgres;Password=Secret!Passw0rd;Database=stock;Port=5432"
      IdGeneratorSettings =
          { GeneratorId = 555
            Epoch = DateTimeOffset.Parse "2020-10-01 12:30:00"
            TimestampBits = byte 41
            GeneratorIdBits = byte 10
            SequenceBits = byte 12 } }

let testRoot = compose testSettings