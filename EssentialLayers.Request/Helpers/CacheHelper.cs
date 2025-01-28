using EssentialLayers.Helpers.Cache;
using System.Net.Http;

namespace EssentialLayers.Request.Helpers
{
	public class CacheHelper<TResult>
	{
		public static readonly SimpleMemoryCache<HttpResponseMessage> HttpResponseMessage = new();
	}
}