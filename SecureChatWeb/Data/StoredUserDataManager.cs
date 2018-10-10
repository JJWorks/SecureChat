using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using SecureChat.Core;
using SecureChatWeb.StoredUserItems;

namespace SecureChatWeb.Data
{
    /// <summary>
    /// Data Manager for the Verified Users.
    /// </summary>
    public class StoredUserDataManager : BaseDataManager
    {
        const string SectionName = "userDataStorage";
        private CacheObject CO;

        /// <summary>
        /// Creates an Instance of Stored User Data Manager.
        /// </summary>
        /// <param name="iconf">Config File.</param>
        public StoredUserDataManager(IConfiguration iconf) : base(SectionName, iconf)
        {
            CO = new CacheObject(SectionName, int.Parse(WCSM.GetSectionConfigValue("CacheLength")));
        }

        /// <summary>
        /// Checks to see if the UserName Matches the Username of the file.
        /// </summary>
        /// <param name="UserHash">Hash User Name to Look up.</param>
        /// <returns>True if in the json file.</returns>
        public bool HasData(string UserHash)
        {
            return StoredUserRetriever.Instance(this.MethodOfRetrieval, CO).HasStoredUser(UserHash);
        }

        /// <summary>
        /// Gets the Stored user information.
        /// </summary>
        /// <param name="UserHash">User Hash to look up.</param>
        /// <returns>A StoredUser of the user.</returns>
        public StoredUser GetStoredUser(string UserHash)
        {
            return StoredUserRetriever.Instance(MethodOfRetrieval, CO).GetStoredUser(UserHash);
        }
    }
}
