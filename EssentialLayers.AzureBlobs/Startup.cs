using EssentialLayers.AzureBlobs.Services.Blob;
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

			return services;
		}

		public static IAzureBlobService ConfigureAzureBlobs(
			this IServiceProvider provider, string connectionString
		)
		{
			using IServiceScope scope = provider.GetRequiredService<IServiceScopeFactory>().CreateScope();
			IAzureBlobService service = scope.ServiceProvider.GetRequiredService<IAzureBlobService>();

			service.SetConnectionString(connectionString);

			return service;
		}
	}
}