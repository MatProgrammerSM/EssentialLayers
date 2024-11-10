using EssentialLayers.Request.Models;
using EssentialLayers.Request.Services.Http;
using EssentialLayers.Request.Services.Request;
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
			services.TryAddScoped<IRequestService, RequestService>();

			return services;
		}

		public static IServiceProvider ConfigureRequest(
			this IServiceProvider provider, HttpOption httpOption
		)
		{
			using IServiceScope scope = provider.GetRequiredService<IServiceScopeFactory>().CreateScope();
			IHttpService httpService = scope.ServiceProvider.GetRequiredService<IHttpService>();
			IRequestService requestService = scope.ServiceProvider.GetRequiredService<IRequestService>();

			httpService.SetOptions(httpOption);
			requestService.SetOptions(httpOption);

			return provider;
		}
	}
}