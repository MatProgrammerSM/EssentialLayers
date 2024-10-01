<image src="https://github.com/MatProgrammerSM/EssentialLayers/blob/master/shared/essential-layers.png" width="50" />

# Essential Layers
Essential layers is a library based in a `way of work functional to start a project from scratch` with a structure designed with Result handling, Cache, Error handling, Blobs, Helpers, Extensions and Web Requests.

It is compatible with almost any project created with . Net in C#, that is, it adapts to Mobile Development projects, Web, API and desktop projects.

### Configure Packages
This solution contains two indpendent packages, to leave to the final user, choosing the way of working, based in their necesities.

### [EssentialLayers](/EssentialLayers/Readme.md)

Available layers to start a project with the most common base functionalities.

#### Helpers

| Layer     | Description |
| :------   | :- |
| [Result](/EssentialLayers/Helpers/Result) | It is used for error handling, results and exception control. |
| [Cache](/EssentialLayers/Helpers/Cache) | The quickest way to implement cache. |
| [Extension](EssentialLayers/Helpers/Extension) | List of methods clasified by data type to extends the functionality and make language more readable. |
| [Logger](/EssentialLayers/Helpers/Logger) | manage the essential methods at the app logger. |

#### Services
| Layer     | Description |
| :------   | :- |
| [Blob](/EssentialLayers/Services/Blob) | Managment of blobs from the app, using different functionalities like read, upload and delete files or containers.  |
| [Token](/EssentialLayers/Services/Token) | To the creation of JWT tokens from a eassier way. |
| [Http](/EssentialLayers/Services/Http) | Is the tipical htttp request, but only with the essential configs. |


### [EssentialLayers.Dapper](/EssentialLayers.Dapper/Readme.md)
Complementary layer to integrate the use of Dapper in your project and add the use of a database SQL Server.

#### Services
| Layer     | Description                   |
| :------   | :- |
| [Estension](/EssentialLayers.Dapper/Estension) | List of methods to extends the functionality of dapper. |
| [Database](/EssentialLayers.Dapper/Services/Database) | List of descriptive methods, to use with stored procedures. |
