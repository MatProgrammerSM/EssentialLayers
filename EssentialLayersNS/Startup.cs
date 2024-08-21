using EssentialLayersNS.Services.HttpService;
using Microsoft.Extensions.DependencyInjection;

namespace EssentialLayersNS
{
	public static class Startup
	{
		public static IServiceCollection AddEssentialLayers(this IServiceCollection services)
		{
			services.AddTransient<IHttpService, HttpWebService>();

			return services;
		}
	}
}