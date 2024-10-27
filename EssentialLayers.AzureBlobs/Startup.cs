using EssentialLayers.AzureBlobs.Services.Blob;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace EssentialLayers.AzureBlobs
{
	public static class Startup
	{
		public static IServiceCollection UseAzureBlob(
			this IServiceCollection services
		)
		{
			services.TryAddScoped<IAzureBlobService, AzureBlobService>();

			return services;
		}

		public static IAzureBlobService ConfigureAzureBlob(
			this IServiceProvider provider, string connectionString
		)
		{
			IAzureBlobService service = provider.GetRequiredService<IAzureBlobService>();

			service.SetConnectionString(connectionString);

			return service;
		}
	}
}