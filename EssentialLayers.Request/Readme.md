# Essential Layers
### EssentialLayers.Request

Is a complement to the package [EssentialLayers](/EssentialLayers/Readme.md) to provide an extra layer for using http requests in an easy way.

### Configure

Add the dependencies in your **Program.cs** file

```
builder.Services.UseRequest();
```

And then set the options (Optional)

```
app.Services.ConfigureRequest(
	new HttpOption
	{
		BaseUri = "https/api.dev",
		AppName = "MyApi",
		AppVersion = "v1",
		CastResultAsResultHelper = true,
		InsensitiveMapping = true
	}
);
```

#### Release Notes
 - It was solved the way of configure globally (ConfigureRequest) in the program file `23-01-2025`
 - Was solved the configuration issues to Http and Request services + RequestHelper + Logs implementation `13/12/2024`
 - It was added a new HttpHelper `12/12/2024`
 - It was removed the TRequest at the method GetAsync + Logs `06/12/2024`
 - Solved issue on serialize() & Added insensitiveMapping with default true `12/11/2024`
 - It was added a content type in a Request and changed the models location + Fixed reponse issue `05/11/2024`
 - It's added "CastResultAsResultHelper" parameter at HttpOption model in HttpService `29/10/2024`

Created by [Mario Soto Moreno](https://github.com/MatProgrammerSM)