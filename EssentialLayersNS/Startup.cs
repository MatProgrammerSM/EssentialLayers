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
			services.AddScoped<IDatabaseService, DatabaseService>();
			services.AddScoped<IHttpWebService, HttpWebService>();
			services.AddScoped<IAzureBlobService, AzureBlobService>();

			return services;
		}
	}
}