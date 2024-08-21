using EssentialLayersNS.Helpers.Cache;
using EssentialLayersNS.Services.HttpService;

namespace EssentialLayers.Helpers.Cache
{
	public abstract class CacheHelper<TResult>
	{
		public static readonly SimpleMemoryCache<HttpWebResponse<TResult>> HttpWebResponseCache = new();
	}
}