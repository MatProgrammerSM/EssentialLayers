using EssentialLayers.AzureBlobs.Helpers;
using EssentialLayers.AzureBlobs.Services.Blob;
using EssentialLayers.AzureBlobs.Services.Connection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace EssentialLayers.AzureBlobs
{
	public static class Startup
	{
		public static IServiceCollection UseAzureBlobs(
			this IServiceCollection services
		)
		{
			services.TryAddScoped<IAzureBlobService, AzureBlobService>();
			services.TryAddSingleton<IConnectionService, ConnectionService>();

			return services;
		}

		public static IServiceProvider ConfigureAzureBlobs(
			this IServiceProvider provider, string connectionString
		)
		{
			IConnectionService service = provider.GetRequiredService<IConnectionService>();

			Tools.Init(provider);

			service.Set(connectionString);

			return provider;
		}
	}
}