﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using SecureChatWeb.Data;
using SecureChatWeb.UserProfile;
using SecureChat.Core.Hash;
using SecureChat.Core.Math;
using Newtonsoft.Json;
using SecureChatWeb.StoredUserItems;

namespace SecureChatWeb.CommunicationHub
{
    public class jjj
    {
        public string UserName;
        public bool Verified = false;
    }
    public class chatHub : Hub
    {

        /// <summary>
        /// Creates an instance of chatHub.
        /// </summary>
        /// <param name="config">Configuration</param>
        public chatHub(IConfiguration config) : base()
        {
            SavedUserInfo = new StoredUserDataManager(config);
            NumberOfSprites = config.GetSection("appsettings").GetValue<int>("NumberOfSprites");
        }

        private int NumberOfSprites;

        private StoredUserDataManager SavedUserInfo;

        private static IDictionary<string, List<UserInformation>> RoomToUsers = new Dictionary<string, List<UserInformation>>();

        private readonly int LowNumber = 1;
        private readonly int MaxNumber = 9999999;

        private const string salt = "It\'s the Bee\'s Knees.  Jeeves555";
        private readonly int NumberofHashIterations = 36;

        /// <summary>
        /// Override Disconnect to remove users from rooms.
        /// </summary>
        /// <param name="exception">Exception.</param>
        /// <returns>Task</returns>
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var RoomsLocated = RoomsConnectionIDisIn(Context.ConnectionId);
            foreach(var Rooms in RoomsLocated)
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, Rooms);
            }
            await ForceDisconnect(Context.ConnectionId);
            await base.OnDisconnectedAsync(exception);
        }


        /// <summary>
        /// Request to Join the Room.
        /// </summary>
        /// <param name="chatRoom">The chat room to join.</param>
        /// <param name="userName">Display Name of the User.</param>
        /// <returns>Int when Function is done.</returns>
        public async Task<int> JoinRoom(string chatRoom, string userName)
        {
            var lowerCaseChat = chatRoom.ToLower();
            bool isNewRoom = false;

            //Create Room
            if(!RoomToUsers.ContainsKey(lowerCaseChat))
            {
                RoomToUsers.Add(lowerCaseChat, new List<UserInformation>());
                isNewRoom = true;
            }

            var currentUserInfo = await AddUsertoRoomAsync(await HashUserNameAsync(userName), userName, lowerCaseChat);

            //Having trouble getting signalr automatically converting complex types.  Hence stringing things.
            await Clients.Caller.SendAsync("loadUsers", JsonConvert.SerializeObject(RoomToUsers[lowerCaseChat].Cast<UserInfoBase>().ToArray()));

            if (!isNewRoom)
            {
                await Clients.GroupExcept(lowerCaseChat, Context.ConnectionId).SendAsync("hubAddUser", JsonConvert.SerializeObject((UserInfoBase)currentUserInfo));
            }
            return 1;
        }



        /// <summary>
        /// Sends a Message to the Chat Room.
        /// </summary>
        /// <param name="ChatRoom">Chat Room to send a message to.</param>
        /// <param name="MessageToSend">Message to send to the chat room.</param>
        /// <returns>Dummy int for async purposes.</returns>
        public async Task<int> SendChat(string ChatRoom, string MessageToSend)
        {
            var lowerCaseChat = ChatRoom.ToLower();
            var UserThatSent = RoomToUsers[lowerCaseChat].Find(x => x.ConnectionID.Equals(Context.ConnectionId));
            var UTCSnap = DateTime.UtcNow;
            int ModnumtoUse = int.Parse(UserThatSent.Identifier) % NumberOfSprites;

            await Clients.Caller.SendAsync("displayMessage", MessageToSend, UserThatSent.Verified, true, UserThatSent.UserName, UTCSnap, ModnumtoUse);
            await Clients.GroupExcept(lowerCaseChat, Context.ConnectionId).SendAsync("displayMessage", MessageToSend, UserThatSent.Verified, false, UserThatSent.UserName, UTCSnap, ModnumtoUse);
            return 1;
        }



        /// <summary>
        /// Forces the disconnect on a connectionID to a particular room or all rooms.
        /// </summary>
        /// <param name="ConnectionIDtoRemove">ConnectionID to Remove from the chatroom.</param>
        /// <param name="Chatroom">Chatroom to remove the connectionID from.</param>
        /// <returns>Dummy int for async purposes.</returns>
        private async Task<int> ForceDisconnect(string ConnectionIDtoRemove, string Chatroom = null)
        {
            IList<string> RoomsLocated = new List<string>();
            if (string.IsNullOrEmpty(Chatroom))
            {
                RoomsLocated = RoomsConnectionIDisIn(ConnectionIDtoRemove);
            }
            else
                RoomsLocated.Add(Chatroom);

            if (RoomsLocated.Count > 0)
            {
                foreach(var roomName in RoomsLocated)
                {
                    if (RoomToUsers.ContainsKey(roomName))
                    {
                        foreach (UserInformation UIM in RoomToUsers[roomName].FindAll(x => x.ConnectionID.Equals(ConnectionIDtoRemove)))
                        {
                            await Clients.Group(roomName).SendAsync("removeUser", UIM.Identifier);
                            RoomToUsers[roomName].Remove(UIM);
                        }
                        await Groups.RemoveFromGroupAsync(ConnectionIDtoRemove, roomName);

                        // Destroy Room if no one is there
                        if (RoomToUsers[roomName].Count <= 0)
                            RoomToUsers.Remove(roomName);
                    }
                }
            }
            return 0;
        }

        /// <summary>
        /// Finds the list of rooms a connectionID is connected to.
        /// </summary>
        /// <param name="ConnectionID">Connection ID</param>
        /// <returns>IList of rooms user is in.</returns>
        private IList<string> RoomsConnectionIDisIn(string ConnectionID)
        {
            var RoomsLocated = new List<string>();

            Parallel.ForEach(RoomToUsers, entry =>
            {
                if (entry.Value.Any(x => x.ConnectionID.Equals(ConnectionID)))
                    RoomsLocated.Add(entry.Key);
            }
            );

            return RoomsLocated;
        }


        /// <summary>
        /// Adds the current context user to a Room.
        /// </summary>
        /// <param name="HashSumUser">Hash SHA user.</param>
        /// <param name="userName">Display Name.</param>
        /// <param name="ChatRoom">Chatroom to join.</param>
        /// <returns>UserInfoBase of the user.</returns>
        private async Task<UserInfoBase> AddUsertoRoomAsync(string HashSumUser, string userName, string ChatRoom)
        {
            UserInformation currentUserInfo = null;
            if (SavedUserInfo.HasData(HashSumUser))
            {
                StoredUser userInFile = SavedUserInfo.GetStoredUser(HashSumUser);
                currentUserInfo = new UserInformation(Context.ConnectionId, userInFile.DisplayName, RandomNumberThread.Instance.Next(LowNumber, MaxNumber).ToString(), true);

            }
            else
            {
                currentUserInfo = new UserInformation(Context.ConnectionId, userName, RandomNumberThread.Instance.Next(LowNumber, MaxNumber).ToString(), false);
            }
            RoomToUsers[ChatRoom].Add(currentUserInfo);
            await Groups.AddToGroupAsync(Context.ConnectionId, ChatRoom);
            return currentUserInfo;
        }

        /// <summary>
        /// Async Running of Hashing users.
        /// </summary>
        /// <param name="UserName">UserName to Hash.</param>
        /// <returns>String of the Hash</returns>
        private Task<string> HashUserNameAsync(string UserName)
        {
            return Task.Run<string>(() => HashUserName(UserName));
        }

        /// <summary>
        /// Hashes a username.
        /// </summary>
        /// <param name="UserName">UserName to Hash.</param>
        /// <returns>Hashsun of username</returns>
        /// <remarks>If you are using this for a website authentication, please note.  This is made to be easy to log into and have a cool icon.</remarks>
        private string HashUserName(string UserName)
        {
            string HashedUserName = UserName + salt;
            for (int i = 0; i < NumberofHashIterations; i++)
            {
                HashedUserName = HashedUserName.ToHashSHA1();

            }
            return HashedUserName;
        }

    }
}
