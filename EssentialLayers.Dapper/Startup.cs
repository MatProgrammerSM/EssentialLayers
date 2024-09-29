using EssentialLayers.Dapper.Services.Database;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace EssentialLayers.Dapper
{
	public static class Startup
	{
		public static IServiceCollection AddEssentialLayersDapper(
			this IServiceCollection services
		)
		{
			services.TryAddSingleton<IDatabaseService, DatabaseService>();

			return services;
		}

		public static IDatabaseService ConfigureDatabase(
			this IServiceProvider provider, string connectionString
		)
		{
			IDatabaseService service = provider.GetRequiredService<IDatabaseService>();

			service.SetConnectionString(connectionString);

			return service;
		}
	}
}