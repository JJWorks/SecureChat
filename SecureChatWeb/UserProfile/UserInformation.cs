using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecureChatWeb.UserProfile
{
    /// <summary>
    /// Sets the User Information.  Base of user InfoBase.
    /// </summary>
    public class UserInformation : UserInfoBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserInformation"/> class.
        /// </summary>
        /// <param name="_ConnectionID">Hubs ConnectionID.</param>
        /// <param name="_UserName">Name of the user.</param>
        public UserInformation(string _ConnectionID, string _UserName, string _Identifier, bool _Verified = false)
        {
            ConnectionID = _ConnectionID;
            base.UserName = _UserName;
            base.Verified = _Verified;
            base.Identifier = _Identifier;
        }

        /// <summary>
        /// Gets the Hub ConnectionID.
        /// </summary>
        /// <value>
        /// The Hub ConnectionID.
        /// </value>
        public string ConnectionID { get; private set; }
    }
}
