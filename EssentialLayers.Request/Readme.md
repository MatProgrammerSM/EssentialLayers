# Essential Layers
### EssentialLayers.Request

Is a complement to the package `EssentialLayers` to provide an extra layer for using http requests in an easy way.

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
		AppName = "AppName",
		AppVersion = "v1"
	}
);
```

Created by [Mario Soto Moreno](https://github.com/MatProgrammerSM)