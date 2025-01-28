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
			services.TryAddSingleton<IHttpService, HttpService>();
			services.TryAddSingleton<IRequestService, RequestService>();

			return services;
		}

		public static IServiceProvider ConfigureRequest(
			this IServiceProvider provider, HttpOption httpOption
		)
		{
			IHttpService httpService = provider.GetRequiredService<IHttpService>();
			IRequestService requestService = provider.GetRequiredService<IRequestService>();

			httpService.SetOptions(httpOption);
			requestService.SetOptions(httpOption);

			return provider;
		}
	}
}