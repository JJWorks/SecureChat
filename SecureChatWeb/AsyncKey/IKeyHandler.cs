using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecureChatWeb.AsyncKey
{
    public interface IKeyHandler
    {

        void AddKey(string Room, string Public);

        void RemoveKey(string Room, string PublicKey);

        string[] GetPublicKeys(string Room);

    }
}
