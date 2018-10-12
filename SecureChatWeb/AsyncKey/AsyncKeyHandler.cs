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
        internal class RoomKeys
        {
            public RoomKeys()
            {
                PublicKeys = new List<string>();
            }


            public int Count
            {
                get { return PublicKeys.Count; }
            }

            public void AddKey(string _newKey)
            {
                if(!PublicKeys.Contains(_newKey))
                {
                    PublicKeys.Add(_newKey);
                }
            }

            public void RemoveKey(string _remove)
            {
                if (PublicKeys.Contains(_remove))
                {
                    PublicKeys.Remove(_remove);
                }
            }

            public bool ContainsKey(string _key)
            {
                return PublicKeys.Contains(_key);
            }

            public string[] GetAllKeys()
            {
                return PublicKeys.ToArray<string>();
            }


            private IList<string> PublicKeys;
        }

        public AsyncKeyHandler()
        {
            RoomConnectionKeys = new Dictionary<string, RoomKeys>();
        }

        private IDictionary<string, RoomKeys> RoomConnectionKeys;


        public void AddRoom(string Room)
        {
            if (!RoomConnectionKeys.ContainsKey(Room))
            {
                RoomConnectionKeys.Add(Room, new RoomKeys());
            }
        }

        public void AddKey(string Room, string Public)
        {
            AddRoom(Room);

            if (!RoomConnectionKeys[Room].ContainsKey(Public))
                RoomConnectionKeys[Room].AddKey(Public);
        }

        public void RemoveKey(string Room, string PublicKey)
        {
            if (RoomConnectionKeys.ContainsKey(Room))
            {
                if(RoomConnectionKeys[Room].ContainsKey(PublicKey))
                {
                    RoomConnectionKeys[Room].RemoveKey(PublicKey);
                }
                
                if (RoomConnectionKeys[Room].Count <= 0)
                    RoomConnectionKeys.Remove(Room);
            }
        }

        public string[] GetPublicKeys(string Room)
        {
            if (!RoomConnectionKeys.ContainsKey(Room))
                return new string[] { };

            return RoomConnectionKeys[Room].GetAllKeys();
        }


    }
}
