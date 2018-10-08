using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using SecureChatWeb.Emoji;
using SecureChat.Core;


namespace SecureChatWeb.Data
{
    /// <summary>
    /// Managers the static data for Emoji Items.
    /// </summary>
    public class EmojiDataManager : BaseDataManager
    {
        /// <summary>
        /// Emoji look up in the configuration file.
        /// </summary>
        const string SectionName = "emojiData";
        
        /// <summary>
        /// Creates an Instance of EmojiDataManager.
        /// </summary>
        /// <param name="iconf">IConfiguration reference to the configuration file.</param>
        public EmojiDataManager(IConfiguration iconf) : base(SectionName, iconf) { }

        /// <summary>
        /// Gets all the Emojis.
        /// </summary>
        /// <returns>List of Emoji (class).</returns>
        public List<SecureChatWeb.Emoji.Emoji> GetAllEmoji()
        {
            return EmojiDataRetriever.Instance(this.MethodOfRetrieval, new CacheObject(SectionName, int.Parse(WCSM.GetSectionConfigValue("CacheLength")))).GetListofEmoji();
        }
    }
}
