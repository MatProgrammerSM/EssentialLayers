Essential Layers 1.0

© MSoto Developer 2024

Steps to use:

1. To use this project you need add the dependecies in your Program.cs file

	builder.Services.AddEssentialLayers();

2. Init each service

	To set Connection String of Azure Blob:

	app.Services.ConfigureAzureBlob(CONNECTION_STRING_BLOBS);

	To set Connection String of Database Service:

	app.Services.ConfigureDatabase(ConnectionString);

	To set App info or bearer token of Web:

	app.Services.ConfigureHttp(
		new HttpWebServiceOption
		{
			AppName = "LicensesApp",
			AppVersion = "1.0",
			BearerToken = Bearer_Token
		}	
	);

	or
										
	app.Services.ConfigureHttpToken(BEARER_TOKEN);