using EssentialLayers.AzureBlobs.Services.Connection;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace EssentialLayers.AzureBlobs.Helpers
{
	internal class Tools
	{
		private static Tools? get;

		private static IServiceProvider? Services { get; set; }

		/**/

		public IConnectionService? Connection => Services!.GetService<IConnectionService>();

		/**/

		public static Tools Get => get ??= new Tools();

		/**/

		public static void Init(IServiceProvider serviceProvider) => Services = serviceProvider;
	}
}