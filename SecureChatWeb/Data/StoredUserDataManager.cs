using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using SecureChat.Core;
using SecureChatWeb.StoredUserItems;

namespace SecureChatWeb.Data
{
    public class StoredUserDataManager : BaseDataManager
    {
        const string SectionName = "userDataStorage";
        private CacheObject CO;
        public StoredUserDataManager(IConfiguration iconf) : base(SectionName, iconf)
        {
            CO = new CacheObject(SectionName, int.Parse(WCSM.GetSectionConfigValue("CacheLength")));
        }

        public bool HasData(string UserHash)
        {
            return StoredUserRetriever.Instance(this.MethodOfRetrieval, CO).HasStoredUser(UserHash);
        }

        public StoredUser GetStoredUser(string UserHash)
        {
            return StoredUserRetriever.Instance(MethodOfRetrieval, CO).GetStoredUser(UserHash);
        }
    }
}
