using System;
using System.Collections.Generic;
using System.Linq;

namespace Chat.Connection.Local
{
    using System.Threading.Tasks;
    using Core.Interfaces;
    using Core.Models;
    using Server;

    class LocalServerService : IServerService
    {
        readonly long _userId;
        readonly Server _server;
        private string _token;
        
        internal LocalServerService(Server server, long userId, string token)
        {
            _server = server;
            _userId = userId;
            _token = token;
        }

        internal void SetToken(string token)
        {
            _token = token;
        }

        async Task VertifyTokenAsync()
        {
            var ok = await _server.VertifyTokenAsync(_userId, _token);
            if(!ok)
                throw new Exception("Failed to vertify token.");
        }

        public IAsyncEnumerable<ChatMessage> GetMessages(GetMessagesRequest request)
        {
            VertifyTokenAsync().Wait();
            return _server.GetMessagesAsync(request)
						  .ToAsyncEnumerable()
						  .SelectMany(list => list.ToAsyncEnumerable());
        }

        public async Task<GetChatroomInfoResponse> GetChatroomInfo(GetChatroomInfoRequest request)
        {
            await VertifyTokenAsync();
            var info = await _server.GetChatroomInfoAsync(request.SenderId, request.ChatroomId);
            return new GetChatroomInfoResponse{Success = true, Chatroom = info};
        }

        public async Task<GetPeopleInfoResponse> GetPeopleInfo(GetPeopleInfoRequest request)
        {
            await VertifyTokenAsync();
            var info = await _server.GetPeopleInfoAsync(request.SenderId, request.TargetId);
            return new GetPeopleInfoResponse{PeopleInfo = info};
        }

        public async Task<MakeFriendResponse> MakeFriend(MakeFriendRequest request)
        {
            await VertifyTokenAsync();
            return await _server.MakeFriendsAsync(request);
        }

        public IAsyncEnumerable<GetDataResponse> GetData(GetDataRequest request)
        {
            VertifyTokenAsync().Wait();
            return _server.GetDataAsync(request);
        }

        public async Task<SendMessageResponse> SendMessageAsync(ChatMessage message)
        {
            await VertifyTokenAsync();
            return await _server.ReceiveNewMessageAsync(message);
        }
    }
}
