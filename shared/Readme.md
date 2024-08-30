Essential Layers 1.0

© MSoto Developer 2024

To use this project you need add this code in your Program.cs file

To use HttpWebHelper add:

builder.Services.AddScoped<HttpWebHelper>();

To use AzureBlobHelper add:

builder.Services.AddScoped(
	x => new AzureBlobHelper(YOUR__CONNECTION_STRING_BLOBS)
);

To use DatabaseHelper add:

builder.Services.AddScoped(
	x => new DatabaseHelper(connectionString)
);