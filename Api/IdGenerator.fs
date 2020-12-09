module IdGenerator 
  open System
  open IdGen  
  [<CLIMutable>]
  type Settings =
    { GeneratorId: int
      Epoch: DateTimeOffset
      TimestampBits: byte
      GeneratorIdBits: byte
      SequenceBits: byte }

  let create settings =
    let structure = IdStructure(settings.TimestampBits, settings.GeneratorIdBits, settings.SequenceBits)
    let options = IdGeneratorOptions(structure, DefaultTimeSource(settings.Epoch))
    let generator = IdGenerator(settings.GeneratorId, options)
    fun() -> generator.CreateId()