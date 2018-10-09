using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecureChatWeb.UserProfile
{
    /// <summary>
    /// User Information Base in the Chat.
    /// </summary>
    public class UserInfoBase
    {
        /// <summary>
        /// Creates an instance of UserInfoBase.
        /// </summary>
        public UserInfoBase() { }

        /// <summary>
        /// Gets a value of whether the user is verified.
        /// </summary>
        /// <value>
        ///   <c>true</c> if verified; otherwise, <c>false</c>.
        /// </value>
        public bool Verified { get; protected set; }

        /// <summary>
        /// Gets the name of the user.
        /// </summary>
        /// <value>
        /// The name of the user.
        /// </value>
        public string UserName { get; protected set; }

        /// <summary>
        /// Gets/Sets the user Style
        /// </summary>
        public string Style { get; set; }


        /// <summary>
        /// Gets the Identifier for the user
        /// </summary>
        public string Identifier { get; protected set; }
    }
}
