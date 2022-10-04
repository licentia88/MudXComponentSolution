using Microsoft.Extensions.Caching.Memory;

namespace MudXComponents.Extensions
{
    internal static class MemoryExtensions
    {
        public static void SetCache<TModel>(this IMemoryCache cache, string key, TModel model, MemoryCacheEntryOptions options)
        {
            cache.Set(key, model, options);
        }

        public static void SetCache<TModel>(this IMemoryCache cache, string key, TModel model)
        {
            cache.SetCache(key, model,new());
        }

        //public static TModel Get<TModel>(this IMemoryCache cache, string key)
        //{
        //   return  cache.Get<TModel>(key);
        //}

      
    }
	 
}

