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
    
    public class EmojiDataRetriever
    {
        private static volatile EmojiDataRetriever instance;
        private static object syncRoot = new Object();

        private EmojiDataRetriever(IRetrievalBase _Method, CacheObject CO)
        {
            RetrievingMethod = _Method;
            CacheParameters = CO;
        }

        private IRetrievalBase RetrievingMethod;
        private CacheObject CacheParameters;

        private IMemoryCache cache;

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


        public List<Emoji> GetListofEmoji()
        {
            UpdateCache(RetrievingMethod.GetData());
            return cache.Get<List<Emoji>>(CacheParameters.CacheName);
        }


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
