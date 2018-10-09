using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using SecureChat.Core;
using SecureChat.Core.FileRetrieval;

namespace SecureChatWeb.StoredUserItems
{
    /// <summary>
    /// StoredUserRetriever and caches objects to reduce IO.
    /// </summary>
    /// <remarks>Gets the Stored User and caches it.</remarks>
    public class StoredUserRetriever
    {

        //Class needs to be eliminated/combined with Emoji and Injected Properly.

        private static volatile StoredUserRetriever instance;
        private static object syncRoot = new Object();

        /// <summary>
        /// Private constructure.  Creates the instance of StoredUserRetriever
        /// </summary>
        /// <param name="_Method">IRetrieval Method of getting the data.</param>
        /// <param name="CO">Cache Object Parameters on how the cache should react.</param>
        private StoredUserRetriever(IRetrievalBase _Method, CacheObject CO)
        {
            RetrievingMethod = _Method;
            CacheParameters = CO;
            cache = new MemoryCache(new MemoryCacheOptions());
        }

        private IRetrievalBase RetrievingMethod;
        private CacheObject CacheParameters;

        private IMemoryCache cache;

        /// <summary>
        /// Gets an instance of StoredUserRetriever
        /// </summary>
        /// <param name="_Method">IRetrieval Method of getting the data.</param>
        /// <param name="CO">Cache Object Parameters on how the cache should react.</param>
        /// <returns>A StoredUserRetriever.</returns>
        public static StoredUserRetriever Instance(IRetrievalBase _Method, CacheObject CO)
        {

            if (instance == null)
            {
                lock (syncRoot)
                {
                    if (instance == null)
                        instance = new StoredUserRetriever(_Method, CO);
                }
            }

            return instance;
        }

        /// <summary>
        /// Gets the List of StoredUser.
        /// </summary>
        /// <returns>Collection of StoredUser.</returns>
        public List<StoredUser> GetListofStoredUser()
        {
            if (cache.Get(CacheParameters.CacheName) == null)
            {
                UpdateCache(RetrievingMethod.GetData());
            }
            return cache.Get<List<StoredUser>>(CacheParameters.CacheName);
        }

        /// <summary>
        /// Checks to see if a Stored User Exist.
        /// </summary>
        /// <param name="HashedUserName">Hash of the Stored user name.</param>
        /// <returns>True if exist.</returns>
        public bool HasStoredUser(string HashedUserName)
        {
            if (cache.Get(CacheParameters.CacheName) == null)
            {
                UpdateCache(RetrievingMethod.GetData());
            }
            var CollectionOfUser = cache.Get<List<StoredUser>>(CacheParameters.CacheName);
            return CollectionOfUser.FindIndex(x => x.UserName.Equals(HashedUserName)) >= 0;
        }

        /// <summary>
        /// Gets the StoredUser from the cache or raw data..
        /// </summary>
        /// <param name="HashedUserName">Hash of the Stored user name.</param>
        /// <returns>StoredUser of the Hash User Name.</returns>
        public StoredUser GetStoredUser(string HashedUserName)
        {
            if (cache.Get(CacheParameters.CacheName) == null)
            {
                UpdateCache(RetrievingMethod.GetData());
            }
            var CollectionOfUser = cache.Get<List<StoredUser>>(CacheParameters.CacheName);
            return CollectionOfUser.Find(x => x.UserName.Equals(HashedUserName));
        }

        /// <summary>
        /// Updates the Cache with StoredUser if cache does not exist or expired.
        /// </summary>
        /// <param name="JsonData">JSON data to cache (stored in Usered User collection).</param>
        private void UpdateCache(string JsonData)
        {

            if (cache.Get(CacheParameters.CacheName) == null)
            {
                var CollectionOfUser = JsonConvert.DeserializeObject<List<StoredUser>>(JsonData);

                var Options = new MemoryCacheEntryOptions();
                Options.AbsoluteExpiration = DateTime.Now.AddHours(CacheParameters.CacheHourSustainment);

                cache.Set<List<StoredUser>>(CacheParameters.CacheName, CollectionOfUser, Options);
            }

        }
    }
}
