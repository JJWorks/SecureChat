using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SecureChat.Core.FileRetrieval;
using Microsoft.Extensions.Configuration;


namespace SecureChatWeb.Data
{
    /// <summary>
    /// Base Data Manager class.
    /// </summary>
    public abstract class BaseDataManager
    {
        /// <summary>
        /// Implements an Instance of BaseDataManager to retrieve Data.
        /// </summary>
        /// <param name="WebSection">Web.config Section to Use.</param>
        /// <param name="UseCloud">Use Azure to pull data.</param>
        public BaseDataManager(string WebSection, IConfiguration iconfig)
        {
            config = iconfig;
            var UseCloud = bool.Parse(config.GetSection("appSettings").GetSection("UseCloud").Value);
            WCSM = new WebConfigurationSectionManager(WebSection);
            if (UseCloud)
            {
                MethodOfRetrieval = new AzureCloudFileRetrieval(config.GetSection("ConnectionStrings").GetSection("AzureLocation").Value, WCSM.GetSectionConfigValue("CloudShare"), WCSM.GetSectionConfigValue("CloudFile"));
            }
            else
            {
                MethodOfRetrieval = new LocalFileRetrieval(WCSM.GetSectionConfigValue("FileLocation"));
            }
        }

        /// <summary>
        /// Web.config simple factory object to retrieve data.
        /// </summary>
        protected WebConfigurationSectionManager WCSM;

        /// <summary>
        /// Retrieval Method.
        /// </summary>
        protected IRetrievalBase MethodOfRetrieval;

        protected IConfiguration config;
    }
}
