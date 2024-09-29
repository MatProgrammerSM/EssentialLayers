# Essential Layers
Essential layers is a library based in a `way of work functional to start a project from scratch` with a structure designed with Result handling, Cache, Error handling, Blobs, Helpers, Extensions and Web Requests.

### Configure

Add the dependencies in your **Program.cs** file
```
builder.Services.AddEssentialLayers();
```

### To use the services provided:
You need to configure an extra code to be available like a dependency in your project.

#### If you want to use a Blob Serivice
You need to set the connection string of Azure blob in your **Program.cs** file.
```
app.Services.ConfigureAzureBlob(CONNECTION_STRING_BLOBS);
```

#### If you want to use the Http Service

You need to set the app info in your **Program.cs** file.
```
app.Services.ConfigureHttp(
    new HttpWebServiceOption {
        AppName = "LicensesApp",
        AppVersion = "1.0",
        BearerToken = Bearer_Token
    }
);
```
Or set when the bearer token have been already initialized.
```
app.Services.ConfigureHttpToken(BEARER_TOKEN);
```
Created by [Mario Soto Moreno](https://github.com/MatProgrammerSM)