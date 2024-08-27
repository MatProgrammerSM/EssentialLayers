using Microsoft.Extensions.DependencyInjection;

namespace EssentialLayers
{
	public static class Startup
	{
		public static IServiceCollection AddEssentialLayers(
			this IServiceCollection services
		)
		{
			return services;
		}
	}
}