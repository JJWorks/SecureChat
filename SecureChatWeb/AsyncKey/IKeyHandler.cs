using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecureChatWeb.AsyncKey
{
    public interface IKeyHandler
    {

        void AddKey(string Room, string ConnectionID, string Public);

        string RemoveKey(string Room, string ConnectionID);

        string[] GetPublicKeys(string Room);


        string GetPublicKeys(string Room, string ConnectionID);
    }
}
