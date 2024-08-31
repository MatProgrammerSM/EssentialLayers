using EssentialLayers.Services.Blob;
using EssentialLayers.Services.Database;
using EssentialLayers.Services.Http;
using Microsoft.Extensions.DependencyInjection;

namespace EssentialLayers
{
	public static class Startup
	{
		public static IServiceCollection AddEssentialLayers(
			this IServiceCollection services
		)
		{
			services.AddSingleton<IDatabaseService, DatabaseService>();
			services.AddSingleton<IHttpWebService, HttpWebService>();
			services.AddSingleton<IAzureBlobService, AzureBlobService>();

			return services;
		}
	}
}