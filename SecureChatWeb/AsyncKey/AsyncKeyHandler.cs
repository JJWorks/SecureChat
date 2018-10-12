using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecureChatWeb.AsyncKey
{
    /// <summary>
    /// Tracker of PGP Keys, Rooms and Connection IDs.
    /// </summary>
    public class AsyncKeyHandler : IKeyHandler
    {
        public AsyncKeyHandler() => RoomConnectionKeys = new Dictionary<string, Dictionary<string, string>>();

        private IDictionary<string, Dictionary<string, string>> RoomConnectionKeys;


        public void AddKey(string Room, string ConnectionID, string Public)
        {
            if(!RoomConnectionKeys.Keys.Contains(Room))
            {
                RoomConnectionKeys.Add(Room, new Dictionary<string, string>());
            }
            var TheReference = RoomConnectionKeys[Room];

            if (TheReference.Keys.Contains(ConnectionID))
                TheReference.Remove(ConnectionID);
            TheReference.Add(ConnectionID, Public);
        }

        public string RemoveKey(string Room, string ConnectionID)
        {
            string PublicKeytoRemove = string.Empty;
            if (RoomConnectionKeys.Keys.Contains(Room))
            {
                var TheReference = RoomConnectionKeys[Room];
                if (TheReference.Keys.Contains(ConnectionID))
                {
                    PublicKeytoRemove = TheReference[ConnectionID];
                    TheReference.Remove(ConnectionID);
                }
                if (TheReference.Count <= 0)
                    RoomConnectionKeys.Remove(Room);
            }
            return PublicKeytoRemove;
        }

        public string[] GetPublicKeys(string Room)
        {
            if (!RoomConnectionKeys.Keys.Contains(Room))
                return new string[] { };

            return RoomConnectionKeys[Room].Values.ToArray<string>();
        }


        public string GetPublicKeys(string Room, string ConnectionID)
        {
            string KeytoFind = string.Empty;
            if (RoomConnectionKeys.Keys.Contains(Room))
            {
                var TheReference = RoomConnectionKeys[Room];
                if (TheReference.Keys.Contains(ConnectionID))
                {
                    KeytoFind = TheReference[ConnectionID];
                }
            }
            return KeytoFind;
        }


    }
}
