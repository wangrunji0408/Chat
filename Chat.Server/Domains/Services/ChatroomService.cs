using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chat.Core.Models;
using Chat.Server.Domains.Entities;
using Chat.Server.Domains.Events.User;
using Chat.Server.Domains.Repositories;
using Chat.Server.Infrastructure.EventBus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Chat.Server.Domains.Services
{
    class ChatroomService
    {
        readonly ILogger _logger;
        readonly IChatroomRepository _chatroomRepo;
        private readonly IUserRepository _userRepo;
        private readonly IEventBus _eventBus;

        public ChatroomService(IServiceProvider provider)
        {
            _logger = provider.GetService<ILoggerFactory>()?
                .CreateLogger<ChatroomService>();
            _chatroomRepo = provider.GetRequiredService<IChatroomRepository>();
            _userRepo = provider.GetRequiredService<IUserRepository>();
            _eventBus = provider.GetRequiredService<IEventBus>();

            _eventBus.GetEventStream<BecameFriendsEvent>().Subscribe(
                async e => await GetOrAddP2PChatroom(e.UserId, e.User2Id));
        }
        
        /// <summary>
        /// 确保创建1号全局聊天室
        /// </summary>
        internal void EnsureGlobalChatroomCreated ()
        {
            if (_chatroomRepo.ContainsIdAsync(1).Result)
                return;
            var chatroom = new Chatroom("Global Chatroom", creatorId: 0);

            _chatroomRepo.Add(chatroom);
            _chatroomRepo.SaveChanges();
        }
        
        internal async Task<Chatroom> GetOrAddP2PChatroom(long user1Id, long user2Id)
        {
            var room = await _chatroomRepo.FindP2PChatroomAsync(user1Id, user2Id);
            if (room != null)
                return room;
            var user1Exist = await _userRepo.ContainsIdAsync(user1Id);
            var user2Exist = await _userRepo.ContainsIdAsync(user2Id);
            if(!user1Exist || !user2Exist)
                throw new ArgumentException("User not exist.");
            room = await _chatroomRepo.NewEmptyChatroomAsync("", creatorId: 0);
            room.AddPeople(user1Id, operatorId: 0);
            room.AddPeople(user2Id, operatorId: 0);
            room.P2PUser1Id = Math.Min(user1Id, user2Id);
            room.P2PUser2Id = Math.Max(user1Id, user2Id);
            await _chatroomRepo.SaveChangesAsync();
            return room;
        }
        
        public async Task<ChatroomResponse> HandleNewMessageAsync(ChatMessage message)
        {
            try
            {
                var chatroom = await _chatroomRepo.FindByIdAsync(message.ChatroomId);
                switch (message.Content.ContentCase)
                {
                    case Content.ContentOneofCase.Text:
                    case Content.ContentOneofCase.Image:
                    case Content.ContentOneofCase.File:
                        chatroom.NewMessage(message);
                        break;
                        
                    case Content.ContentOneofCase.New:
                        var newArgs = message.Content.New;
                        chatroom = await NewChatroomAsync(newArgs.PeopleIds, newArgs.Name, message.SenderId);
                        return new ChatroomResponse{Success = true, ChatroomId = chatroom.Id};
                        
                    case Content.ContentOneofCase.Dismiss:
                        await DismissChatroom(message.ChatroomId, message.SenderId);
                        break;
                        
                    case Content.ContentOneofCase.AddPeople:
                        var addArgs = message.Content.AddPeople;
                        await AddPeoplesToChatroom(message.ChatroomId, addArgs.PeopleIds, message.SenderId);
                        break;
                        
                    case Content.ContentOneofCase.RemovePeople:
                        var removeArgs = message.Content.AddPeople;
                        await RemovePeoplesFromChatroom(message.ChatroomId, removeArgs.PeopleIds, message.SenderId);
                        break;
                        
                    case Content.ContentOneofCase.Apply:
                        break;
                    case Content.ContentOneofCase.Quit:
                        var quitArgs = message.Content.Quit;
                        chatroom.Quit(message.SenderId);
                        await _chatroomRepo.SaveChangesAsync();
                        break;

                    case Content.ContentOneofCase.Announce:
                        break;
                    case Content.ContentOneofCase.SetPeoperty:
                        break;
                    case Content.ContentOneofCase.Withdraw:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            catch (Exception e)
            {
                return new ChatroomResponse
                {
                    Success = false,
                    Detail = e.Message
                };
            }
            return new ChatroomResponse {Success = true};
        }

        internal async Task<Chatroom> NewChatroomAsync(IEnumerable<long> peopleIds, string name, long creatorId)
        {
            if(!peopleIds.All(id => _userRepo.ContainsIdAsync(id).Result))
                throw new ArgumentException("User not exist.");
            var chatroom = await _chatroomRepo.NewEmptyChatroomAsync(name, creatorId);
            foreach (var user in peopleIds)
                chatroom.AddPeople(user, creatorId);
            await _chatroomRepo.SaveChangesAsync();
            return chatroom;
        }
        
        internal async Task AddPeoplesToChatroom(long chatroomId, IEnumerable<long> peopleIds, long operatorId)
        {
            if(!peopleIds.All(id => _userRepo.ContainsIdAsync(id).Result))
                throw new ArgumentException("User not exist.");
            var chatroom = await _chatroomRepo.GetByIdAsync(chatroomId);
            foreach (var userId in peopleIds)
                chatroom.AddPeople(userId, operatorId);
            await _chatroomRepo.SaveChangesAsync();
        }
        
        internal async Task RemovePeoplesFromChatroom(long chatroomId, IEnumerable<long> peopleIds, long operatorId)
        {
            if(!peopleIds.All(id => _userRepo.ContainsIdAsync(id).Result))
                throw new ArgumentException("User not exist.");
            var chatroom = await _chatroomRepo.GetByIdAsync(chatroomId);
            foreach (var userId in peopleIds)
                chatroom.RemovePeople(userId, operatorId);
            await _chatroomRepo.SaveChangesAsync();
        }
        
        internal async Task DismissChatroom(long chatroomId, long operatorId)
        {
            var chatroom = await _chatroomRepo.GetByIdAsync(chatroomId);
            chatroom.DismissBy(operatorId);
            await _chatroomRepo.SaveChangesAsync();
        }
    }
}