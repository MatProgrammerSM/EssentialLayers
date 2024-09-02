using EssentialLayers.Helpers.Extension;
using EssentialLayers.Services.Blob;
using EssentialLayers.Services.Database;
using EssentialLayers.Services.Http;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace EssentialLayers
{
	public static class Startup
	{
		public static IServiceCollection AddEssentialAzureBlob(
			this IServiceCollection services, Action<string> configure = null
		)
		{
			services.AddSingleton<IAzureBlobService, AzureBlobService>();

			if (configure.NotNull()) services.Configure(configure);

			return services;
		}
		
		public static IServiceCollection AddEssentialDatabase(
			this IServiceCollection services, Action<HttpWebServiceOption> configure = null
		)
		{
			services.AddSingleton<IDatabaseService, DatabaseService>();

			if (configure.NotNull()) services.Configure(configure);
			
			return services;
		}

		public static IServiceCollection AddEssentialHttp(
			this IServiceCollection services, Action<HttpWebServiceOption> configure = null
		)
		{
			services.AddSingleton<IHttpWebService, HttpWebService>();

			if (configure.NotNull()) services.Configure(configure);

			return services;
		}
	}
}