using EssentialLayers.Dapper.Helpers;
using EssentialLayers.Dapper.Services.Connection;
using EssentialLayers.Dapper.Services.Procedure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace EssentialLayers.Dapper
{
	public static class Startup
	{
		public static IServiceCollection UseDapper(
			this IServiceCollection services
		)
		{
			services.TryAddScoped<IProcedureService, ProcedureService>();
			services.TryAddSingleton<IConnectionService, ConnectionService>();

			return services;
		}

		public static IServiceProvider ConfigureDapper(
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