### To use the services provided:
You need to configure an extra code to be available like a dependency in your project.
```
app.Services.ConfigureAzureBlob(CONNECTION_STRING_BLOBS);
```
#### If you want to use a Blob Serivice
You need to set the connection string of Azure blob in your **Program.cs** file.
```
app.Services.ConfigureAzureBlob(CONNECTION_STRING_BLOBS);
```
Created by [Mario Soto Moreno](https://github.com/MatProgrammerSM)