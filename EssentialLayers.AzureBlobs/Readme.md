# Essential Layers
### EssentialLayers.AzureBlobs

Is a complement to the package [EssentialLayers](/EssentialLayers/Readme.md) to provide an extra layer to use Azure Blobs in an easy way.

```
build.Services.UseAzureBlobs();
```

#### If you want to use a Blob Serivice
You need to set the connection string of Azure blob in your **Program.cs** file.

```
app.Services.ConfigureAzureBlobs(CONNECTION_STRING_BLOBS);
```

#### Release Notes
 - Now, all methods can be called or used as a BlobServiceHelper passing the connectionString at the constructor `12/12/2024`
 - It was added ConnectionString  as a singleton service + Updated libraries `06/12/2024`
 - New service to work with Azure Blobs `27/10/2024`

Created by [Mario Soto Moreno](https://github.com/MatProgrammerSM)