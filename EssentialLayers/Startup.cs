using EssentialLayers.Services.Blob;
using EssentialLayers.Services.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace EssentialLayers
{
	public static class Startup
	{
		public static IServiceCollection AddEssentialLayers(
			this IServiceCollection services
		)
		{
			services.TryAddSingleton<IAzureBlobService, AzureBlobService>();
			services.TryAddSingleton<IHttpWebService, HttpWebService>();

			return services;
		}

		public static IAzureBlobService ConfigureAzureBlob(
			this IServiceProvider provider, string connectionString = null
		)
		{
			IAzureBlobService service = provider.GetRequiredService<IAzureBlobService>();

			service.SetConnectionString(connectionString);

			return service;
		}

		public static IHttpWebService ConfigureHttp(
			this IServiceProvider provider, HttpWebServiceOption options
		)
		{
			IHttpWebService service = provider.GetRequiredService<IHttpWebService>();

			service.SetOptions(options);

			return service;
		}

		public static IHttpWebService ConfigureHttpToken(
			this IServiceProvider provider, string bearerToken
		)
		{
			IHttpWebService service = provider.GetRequiredService<IHttpWebService>();

			service.SetToken(bearerToken);

			return service;
		}
	}
}