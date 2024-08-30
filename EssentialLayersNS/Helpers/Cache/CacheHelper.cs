using EssentialLayers.Services.Http;

namespace EssentialLayers.Helpers.Cache
{
	internal abstract class CacheHelper<TResult>
	{
		public static readonly SimpleMemoryCache<HttpWebResponse<TResult>> HttpWebResponseCache = new();
	}
}