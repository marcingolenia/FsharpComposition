module Settings

[<CLIMutable>]
type Settings = {
    IdGeneratorSettings: IdGenerator.Settings
    SqlConnectionString: string
}