using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecureChatWeb.StoredUserItems
{
    /// <summary>
    /// Stored User Information Class.
    /// </summary>
    public class StoredUser : ICloneable
    {
        /// <summary>
        /// User Name of the Stored User.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Display Name of the Stored user.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Styling of the Stored User.
        /// </summary>
        public string Style { get; set; }

        /// <summary>
        /// Clone Stored User.
        /// </summary>
        /// <returns>Clone of Object.</returns>
        public object Clone()
        {
            return base.MemberwiseClone();
        }
    }
}
