using EssentialLayers.Helpers.Mapper;
using EssentialLayers.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace EssentialLayers
{
	public static class Startup
	{
		public static IServiceCollection UseAzureBlobs(
			this IServiceCollection services
		)
		{
			services.TryAddScoped<IMapperService, MapperService>();

			services.AddSingleton<MapperHelper>();

			return services;
		}
	}
}