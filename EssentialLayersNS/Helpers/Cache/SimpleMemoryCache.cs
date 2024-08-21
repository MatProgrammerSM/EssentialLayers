using EssentialLayers.Helpers.Extension;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading.Tasks;

namespace EssentialLayers.Helpers.Cache
{
	public class SimpleMemoryCache<TItem>
	{
		private readonly MemoryCache MemoryCache;

		public SimpleMemoryCache(MemoryCacheOptions options = null)
		{
			if (options.IsNull())
			{
				options = new MemoryCacheOptions
				{
					ExpirationScanFrequency = TimeSpan.FromMinutes(15)
				};
			}

			MemoryCache = new MemoryCache(options!);
		}

		public async Task<TItem> GetOrCreate(object key, Func<Task<TItem>> createItem)
		{
			if (MemoryCache.TryGetValue(key, out TItem cacheEntry))
			{
				return cacheEntry!;
			}

			cacheEntry = await createItem();

			MemoryCache.Set(key, cacheEntry);

			return cacheEntry;
		}

		public TItem GetOrCreate(object key, TItem item)
		{
			if (MemoryCache.TryGetValue(key, out TItem cacheEntry))
			{
				return cacheEntry!;
			}

			cacheEntry = item;

			MemoryCache.Set(key, cacheEntry);

			return cacheEntry;
		}

		public void Remove(string key)
		{
			MemoryCache.Remove(key);
		}
	}
}