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
    }
}