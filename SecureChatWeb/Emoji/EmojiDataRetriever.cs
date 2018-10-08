using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SecureChat.Core;
using SecureChat.Core.FileRetrieval;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System.Text.Encodings.Web;

namespace SecureChatWeb.Emoji
{
    
    /// <summary>
    /// EmojiDataRetriever and caches objects to reduce IO.
    /// </summary>
    /// <remarks>Gets the Emoji and caches it.</remarks>
    public class EmojiDataRetriever
    {
        private static volatile EmojiDataRetriever instance;
        private static object syncRoot = new Object();

        /// <summary>
        /// Private constructure.  Creates the instance of EmojiRetriever
        /// </summary>
        /// <param name="_Method">IRetrieval Method of getting the data.</param>
        /// <param name="CO">Cache Object Parameters on how the cache should react.</param>
        private EmojiDataRetriever(IRetrievalBase _Method, CacheObject CO)
        {
            RetrievingMethod = _Method;
            CacheParameters = CO;
            cache = new MemoryCache(new MemoryCacheOptions());
        }

        private IRetrievalBase RetrievingMethod;
        private CacheObject CacheParameters;

        private IMemoryCache cache;

        /// <summary>
        /// Gets an instance of EmojiDataRetriever
        /// </summary>
        /// <param name="_Method">IRetrieval Method of getting the data.</param>
        /// <param name="CO">Cache Object Parameters on how the cache should react.</param>
        /// <returns>A EmojiDataRetriever.</returns>
        public static EmojiDataRetriever Instance(IRetrievalBase _Method, CacheObject CO)
        {

            if (instance == null)
            {
                lock (syncRoot)
                {
                    if (instance == null)
                        instance = new EmojiDataRetriever(_Method, CO);
                }
            }

            return instance;

        }

        /// <summary>
        /// Gets the List of Emoji and Commands.
        /// </summary>
        /// <returns>Collection of Emoji.</returns>
        public List<Emoji> GetListofEmoji()
        {
            if (cache.Get(CacheParameters.CacheName) == null)
            {
                UpdateCache(RetrievingMethod.GetData());
            }
            return cache.Get<List<Emoji>>(CacheParameters.CacheName);
        }

        /// <summary>
        /// Updates the Cache with Emoji if cache does not exist or expired.
        /// </summary>
        /// <param name="JsonData">JSON data to cache (stored in Emoji collection).</param>
        private void UpdateCache(string JsonData)
        {
            if (cache.Get(CacheParameters.CacheName) == null)
            {
                List<Emoji> CollectionOfEmoji = JsonConvert.DeserializeObject<List<Emoji>>(JsonData);
                CollectionOfEmoji.Sort((x, y) => x.order.CompareTo(y.order));

                Parallel.ForEach(CollectionOfEmoji, currentEmote =>
                {
                    Parallel.For(0, currentEmote.codes.Length, index =>
                        currentEmote.codes[index] = HtmlEncoder.Default.Encode(currentEmote.codes[index]));
                });
                var Options = new MemoryCacheEntryOptions();
                Options.AbsoluteExpiration = DateTime.Now.AddHours(CacheParameters.CacheHourSustainment);
                
                cache.Set<List<Emoji>>(CacheParameters.CacheName, CollectionOfEmoji, Options);
            }

        }
    }
}
