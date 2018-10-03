using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecureChatWeb.Emoji
{
    /// <summary>
    /// Emoji Class Properties
    /// </summary>
    public class Emoji : ICloneable
    {
        /// <summary>
        /// Emoji Title.
        /// </summary>
        public string title { get; set; }

        /// <summary>
        /// Emoji Image File.
        /// </summary>
        public string imagefile { get; set; }
        
        /// <summary>
        /// Emoji Codes.
        /// </summary>
        public string[] codes { get; set; }


        public int order { get; set; }

        /// <summary>
        /// Clone
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
