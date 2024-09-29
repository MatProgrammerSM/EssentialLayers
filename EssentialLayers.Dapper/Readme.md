# Essential Layers
`EssentialLayers.Dapper` is a complement to the package `EssentialLayers` to offer an extra layer with the ORM dapper, where the main purpose will be, write the business logic in the "stored procedures" using templates that receiving input parameters and return a result set, Currently is just compatible with SQL Server.

### Configure

Add the dependencies in your **Program.cs** file
```
builder.Services.AddEssentialLayersDapper();
```
And then set the connection string

```
app.Services.ConfigureDatabase(Connection_String);
```

Created by [Mario Soto Moreno](https://github.com/MatProgrammerSM)