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
        internal LocalServerService(Server server, long userId)
        {
            _server = server;
            _userId = userId;
        }


        public IAsyncEnumerable<ChatMessage> GetMessages(GetMessagesRequest request)
        {
            return _server.GetMessagesAsync(request)
						  .ToAsyncEnumerable()
						  .SelectMany(list => list.ToAsyncEnumerable());
        }

        public async Task<GetChatroomInfoResponse> GetChatroomInfo(GetChatroomInfoRequest request)
        {
            var info = await _server.GetChatroomInfoAsync(request.SenderId, request.ChatroomId);
            return new GetChatroomInfoResponse{Success = true, Chatroom = info};
        }

        public async Task<GetPeopleInfoResponse> GetPeopleInfo(GetPeopleInfoRequest request)
        {
            var info = await _server.GetPeopleInfoAsync(request.SenderId, request.TargetId);
            return new GetPeopleInfoResponse{PeopleInfo = info};
        }

        public Task<MakeFriendResponse> MakeFriend(MakeFriendRequest request)
        {
            return _server.MakeFriendsAsync(request);
        }

        public IAsyncEnumerable<GetDataResponse> GetData(GetDataRequest request)
        {
            return _server.GetDataAsync(request);
        }

        public async Task SendMessageAsync(ChatMessage message)
        {
            var response = await _server.ReceiveNewMessageAsync(message);
            if(!response.Success)
                throw new Exception($"Failed to send message. {response.Detail}");
        }
    }
}
