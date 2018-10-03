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
        /// Hard Coded Value in Web.config.
        /// </summary>
        const string SectionName = "emojiData";
        
        /// <summary>
        /// 
        /// </summary>
        public EmojiDataManager(IConfiguration iconf) : base(SectionName, iconf) { }

        public List<SecureChatWeb.Emoji.Emoji> GetAllEmoji()
        {
            return EmojiDataRetriever.Instance(this.MethodOfRetrieval, new CacheObject(SectionName, int.Parse(WCSM.GetSectionConfigValue("CacheLength")))).GetListofEmoji();
        }
    }
}
