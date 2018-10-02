using System;
using System.Collections.Generic;
using System.Text;

namespace SecureChat.Core
{
    /// <summary>
    /// A reference object to keep in cache.  Sets the property of the Name and persistency.
    /// </summary>
    public class CacheObject
    {
        /// <summary>
        /// Creates an Instance of CacheObject
        /// </summary>
        /// <param name="_cacheName">Cache Name to retrieve.</param>
        /// <param name="Hours">Hours of persistency.</param>
        public CacheObject(string _cacheName, int Hours)
        {
            CacheHourSustainment = Hours;
            CacheName = _cacheName;
        }
        /// <summary>
        /// Hours of how long the cache should sustain.
        /// </summary>
        public int CacheHourSustainment { get; private set; }

        /// <summary>
        /// Cache Name
        /// </summary>
        public string CacheName { get; private set; }

    }
}
