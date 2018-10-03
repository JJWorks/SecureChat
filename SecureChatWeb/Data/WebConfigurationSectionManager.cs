using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Collections.Specialized;

namespace SecureChatWeb.Data
{
    /// <summary>
    /// Grabs Sections from Web.config.
    /// </summary>
    public class WebConfigurationSectionManager
    {
        /// <summary>
        /// Implements WebConfigurationManager to Get Name Values of a section.
        /// </summary>
        /// <param name="SectionName">Section Name in Web.config.</param>
        public WebConfigurationSectionManager(string SectionName)
        {
            configValues = _config.GetSection(SectionName) as NameValueCollection;
        }

        private IConfiguration _config;

        /// <summary>
        /// Name Value Collection From Web.config.
        /// </summary>
        private NameValueCollection configValues;

        /// <summary>
        /// Gets a value from the section.
        /// </summary>
        /// <param name="Key">Key in web.config to retreive.</param>
        /// <returns>The value in web.config.</returns>
        public string GetSectionConfigValue(string Key)
        {
            return configValues[Key];
        }
    }
}
