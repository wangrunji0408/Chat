using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Chat.Core.Models;

namespace Chat.Core.Interfaces
{
    public interface IServerService
    {
        Task<SendMessageResponse> SendMessageAsync(ChatMessage message);
        IAsyncEnumerable<ChatMessage> GetMessages(GetMessagesRequest request);
        Task<GetChatroomInfoResponse> GetChatroomInfo(GetChatroomInfoRequest request);
        Task<GetPeopleInfoResponse> GetPeopleInfo(GetPeopleInfoRequest request);
        Task<MakeFriendResponse> MakeFriend(MakeFriendRequest request);
        IAsyncEnumerable<GetDataResponse> GetData(GetDataRequest request);
    }
}
