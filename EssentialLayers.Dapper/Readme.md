# Essential Layers

### EssentialLayers.Dapper

Is a complement to the package `EssentialLayers` to provide an extra layer with the ORM dapper, where the main purpose will be, write the business logic in the "stored procedures" using templates that receiving input parameters and return a result set, Currently is just compatible with SQL Server.

### Configure

Add the dependencies in your **Program.cs** file

```
builder.Services.UseDapper();
```

And then set the connection string

```
app.Services.ConfigureDapper(Connection_String);
```

#### Release Notes
 - Added SetConnection to set the runtime value `24/01/2025`

Created by [Mario Soto Moreno](https://github.com/MatProgrammerSM)