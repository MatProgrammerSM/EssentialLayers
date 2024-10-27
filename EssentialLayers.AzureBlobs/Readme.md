# Essential Layers
`EssentialLayers.AzureBlobs` is a complement to the package `EssentialLayers` to provide an extra layer to use Azure Blobs in an easy way.
```
build.Services.UseAzureBlob();
```

#### If you want to use a Blob Serivice
You need to set the connection string of Azure blob in your **Program.cs** file.

```
app.Services.ConfigureAzureBlob(CONNECTION_STRING_BLOBS);
```

Created by [Mario Soto Moreno](https://github.com/MatProgrammerSM)