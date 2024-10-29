using EssentialLayers.Request.Services.Http;
using EssentialLayers.Request.Services.Http.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace EssentialLayers.Request
{
	public static class Startup
	{
		public static IServiceCollection UseRequest(
			this IServiceCollection services
		)
		{
			services.TryAddScoped<IHttpService, HttpService>();

			return services;
		}

		public static IHttpService ConfigureRequest(
			this IServiceProvider provider, HttpOption httpOption
		)
		{
			using IServiceScope scope = provider.GetRequiredService<IServiceScopeFactory>().CreateScope();
			IHttpService service = scope.ServiceProvider.GetRequiredService<IHttpService>();

			service.SetOptions(httpOption);

			return service;
		}
	}
}