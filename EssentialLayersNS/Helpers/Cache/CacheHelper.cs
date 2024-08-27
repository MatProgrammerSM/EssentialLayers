using EssentialLayers.Helpers.Http;

namespace EssentialLayers.Helpers.Cache
{
    public abstract class CacheHelper<TResult>
	{
		public static readonly SimpleMemoryCache<HttpWebResponse<TResult>> HttpWebResponseCache = new();
	}
}