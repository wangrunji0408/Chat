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
        public long UserId { get; internal set; }
        public long RoomId { get; internal set; }
        internal IServerService ServerService { get; set; }

        void ThrowIfFailed(SendMessageResponse response)
        {
            if(!response.Success)
                throw new Exception($"Failed. {response.Detail}");
        }
        
        public async Task SendTextAsync(string text)
        {
            var message = new ChatMessage
            {
                SenderId = UserId,
                ChatroomId = RoomId,
                Content = new Content {Text = text}
            };
            ThrowIfFailed(await ServerService.SendMessageAsync(message));
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
            ThrowIfFailed(await ServerService.SendMessageAsync(message));
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
            ThrowIfFailed(await ServerService.SendMessageAsync(message));
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
            ThrowIfFailed(await ServerService.SendMessageAsync(message));
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
        
        public async Task<Chatroom> NewChatroom(IEnumerable<long> peopleIds, string name = "")
        {
            var message = new ChatMessage
            {
                SenderId = UserId,
                ChatroomId = RoomId,
                Content = new Content
                {
                    New = new Content.Types.New{PeopleIds = { peopleIds}, Name = name}
                }
            };
            var rsp = await ServerService.SendMessageAsync(message);
            ThrowIfFailed(rsp);
            return new Chatroom
            {
                RoomId = rsp.ChatroomId,
                UserId = UserId,
                ServerService = ServerService
            };
        }
        
        public async Task QuitAsync()
        {
            var message = new ChatMessage
            {
                SenderId = UserId,
                ChatroomId = RoomId,
                Content = new Content
                {
                    Quit = new Content.Types.Quit()
                }
            };
            ThrowIfFailed(await ServerService.SendMessageAsync(message));
        }

        public async Task ChangeNameAsync(string value)
        {
            await SetPropertyAsync("Name", value);
        }
        
        async Task SetPropertyAsync(string key, string value)
        {
            var message = new ChatMessage
            {
                SenderId = UserId,
                ChatroomId = RoomId,
                Content = new Content 
                { 
                    SetPeoperty = new Content.Types.SetProperty { Key = key, Value = value } 
                }
            };
            ThrowIfFailed(await ServerService.SendMessageAsync(message));
        }
    }
}