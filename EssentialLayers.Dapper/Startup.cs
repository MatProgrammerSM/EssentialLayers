﻿using EssentialLayers.Dapper.Services.Procedure;
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

			return services;
		}

		public static IProcedureService ConfigureDapper(
			this IServiceProvider provider, string connectionString
		)
		{
			IProcedureService service = provider.GetRequiredService<IProcedureService>();

			service.SetConnectionString(connectionString);

			return service;
		}
	}
}