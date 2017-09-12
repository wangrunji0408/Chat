using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chat.Core.Interfaces;
using Chat.Core.Models;

namespace Chat.Client
{
    public class Chatroom
    {
        internal long UserId { get; set; }
        internal long RoomId { get; set; }
        internal IServerService ServerService { get; set; }
        
        public async Task SendTextAsync(string text)
        {
            var message = new ChatMessage
            {
                SenderId = UserId,
                ChatroomId = RoomId,
                Content = new Content {Text = text}
            };
            await ServerService.SendMessageAsync(message);
        }
        
        public async Task DismissAsync()
        {
            var message = new ChatMessage
            {
                SenderId = UserId,
                ChatroomId = RoomId,
                Content = new Content
                {
                    Dismiss = new Content.Types.Dismiss()
                }
            };
            await ServerService.SendMessageAsync(message);
        }

        public async Task AddPeoplesAsync(IEnumerable<long> peopleIds)
        {
            var message = new ChatMessage
            {
                SenderId = UserId,
                ChatroomId = RoomId,
                Content = new Content
                {
                    AddPeople = new Content.Types.AddPeople{ PeopleIds = { peopleIds } }
                }
            };
            await ServerService.SendMessageAsync(message);
        }
        
        public async Task RemovePeoplesAsync(IEnumerable<long> peopleIds)
        {
            var message = new ChatMessage
            {
                SenderId = UserId,
                ChatroomId = RoomId,
                Content = new Content
                {
                    RemovePeople = new Content.Types.RemovePeople{ PeopleIds = { peopleIds } }
                }
            };
            await ServerService.SendMessageAsync(message);
        }
        
        public async Task<ChatroomInfo> GetInfoAsync()
        {
            var request = new GetChatroomInfoRequest
            {
                SenderId = UserId,
                ChatroomId = RoomId
            };
            var response = await ServerService.GetChatroomInfo(request);
            if(response.Success == false)
                throw new Exception($"Failed to get chatroom info. {response.Detail}");
            return response.Chatroom;
        }

        public async Task<List<ChatMessage>> GetRecentMessagesAsync(int count)
        {
            var request = new GetMessagesRequest
            {
                SenderId = UserId,
                ChatroomId = RoomId,
                AfterTimeUnixMs = 0,
                Count = count
            };
            return await ServerService.GetMessages(request).ToList();
        }
    }
}