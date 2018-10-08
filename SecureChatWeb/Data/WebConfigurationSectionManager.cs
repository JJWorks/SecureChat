using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Collections.Specialized;

namespace SecureChatWeb.Data
{
    /// <summary>
    /// Grabs Sections from the configuration.
    /// </summary>
    public class WebConfigurationSectionManager
    {
        /// <summary>
        /// Implements WebConfigurationManager to Get Name Values of a section.
        /// </summary>
        /// <param name="SectionName">Section Name in Web.config.</param>
        /// <param name="_config">IConfiguration of the appsettings.</param>
        public WebConfigurationSectionManager(string SectionName, IConfiguration _config)
        {
            configValues = _config.GetSection(SectionName)
                    .GetChildren().ToDictionary(x => x.Key, x => x.Value);
        }
        

        /// <summary>
        /// Dictionary Collection From Web.config.
        /// </summary>
        private IDictionary<string, string> configValues;

        /// <summary>
        /// Gets a value from the section.
        /// </summary>
        /// <param name="Key">Key in configuration to retreive.</param>
        /// <returns>The value in configuration.</returns>
        public string GetSectionConfigValue(string Key)
        {
            return configValues[Key];
        }
    }
}
