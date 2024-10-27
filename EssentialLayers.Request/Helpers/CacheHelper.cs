using EssentialLayers.Helpers.Cache;
using EssentialLayers.Request.Services.Http;
using System.Net.Http;

namespace EssentialLayers.Request.Helpers
{
	internal class CacheHelper<TResult>
	{
		public static readonly SimpleMemoryCache<HttpResponseMessage> HttpResponseMessage = new();
	}
}