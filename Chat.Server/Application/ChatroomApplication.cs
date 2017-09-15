using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chat.Core.Models;
using Chat.Server.Domains.Entities;
using Chat.Server.Domains.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.Server.Application
{
    public class ChatroomApplication
    {
        public long ChatroomId { get; internal set; }
        public long OperatorId { get; internal set; }
        
        readonly IUserRepository _userRepo;
        readonly IChatroomRepository _chatroomRepo;
        readonly IServiceProvider _provider;

        internal ChatroomApplication(IServiceProvider provider)
        {
            _provider = provider;
            _userRepo = provider.GetRequiredService<IUserRepository>();
            _chatroomRepo = provider.GetRequiredService<IChatroomRepository>();
        }

        internal async Task NewMessageAsync(ChatMessage message)
        {
            var chatroom = await _chatroomRepo.GetByIdAsync(ChatroomId);
            chatroom.NewMessage(message);
        }
        
        public async Task<ChatroomApplication> NewChatroomAsync(IEnumerable<long> peopleIds, string name = "")
        {
            peopleIds = peopleIds.ToArray();
            if (OperatorId != 0)
            {
                var user = await _userRepo.GetByIdAsync(OperatorId);
                if(!peopleIds.Contains(user.Id))
                    throw new ArgumentException("All users must contain yourself.");
                if(!peopleIds.All(id => id == user.Id || user.IsFriend(id)))
                    throw new ArgumentException("All users must be your friend.");
            }
            
            var chatroom = await _chatroomRepo.NewEmptyChatroomAsync(name, OperatorId);
            foreach (var userId in peopleIds)
                chatroom.AddPeople(userId, 0);
            if(OperatorId != 0)
                chatroom.SetRole(OperatorId, UserChatroom.UserRole.Admin, 0);
            await _chatroomRepo.SaveChangesAsync();
            return new ChatroomApplication(_provider)
            {
                ChatroomId = chatroom.Id,
                OperatorId = OperatorId
            };
        }
        
        public async Task AddPeoplesAsync(IEnumerable<long> peopleIds)
        {
            if(!peopleIds.All(id => _userRepo.ContainsIdAsync(id).Result))
                throw new ArgumentException("User not exist.");
            var chatroom = await _chatroomRepo.GetByIdAsync(ChatroomId);
            foreach (var userId in peopleIds)
                chatroom.AddPeople(userId, OperatorId);
            await _chatroomRepo.SaveChangesAsync();
        }
	    
        public async Task AddPeopleAsync(long userId)
        {
            await AddPeoplesAsync(new []{userId});
        }
        
        public async Task RemovePeoplesAsync(IEnumerable<long> peopleIds)
        {
            if(!peopleIds.All(id => _userRepo.ContainsIdAsync(id).Result))
                throw new ArgumentException("User not exist.");
            var chatroom = await _chatroomRepo.GetByIdAsync(ChatroomId);
            foreach (var userId in peopleIds)
                chatroom.RemovePeople(userId, OperatorId);
            await _chatroomRepo.SaveChangesAsync();
        }
	    
        public async Task RemovePeopleAsync(long userId)
        {
            await RemovePeoplesAsync(new []{userId});
        }
        
        public async Task DismissAsync()
        {
            var chatroom = await _chatroomRepo.GetByIdAsync(ChatroomId);
            chatroom.DismissBy(OperatorId);
            await _chatroomRepo.SaveChangesAsync();
        }

        public async Task QuitAsync()
        {
            var chatroom = await _chatroomRepo.GetByIdAsync(ChatroomId);
            chatroom.Quit(OperatorId);
            await _chatroomRepo.SaveChangesAsync();
        }

        public async Task SetRoleAsync(long userId, string role)
        {
            var chatroom = await _chatroomRepo.GetByIdAsync(ChatroomId);
            var roleEnum = (UserChatroom.UserRole)Enum.Parse(typeof(UserChatroom.UserRole), role);
            chatroom.SetRole(userId, roleEnum, OperatorId);
            await _chatroomRepo.SaveChangesAsync();
        }
        
        public async Task ChangeNameAsync(string value)
        {
            var chatroom = await _chatroomRepo.GetByIdAsync(ChatroomId);
            chatroom.SetName(value, OperatorId);
            await _chatroomRepo.SaveChangesAsync();
        }
        
        public async Task BlockAsync(long userId)
        {
            var chatroom = await _chatroomRepo.GetByIdAsync(ChatroomId);
            chatroom.Block(userId, OperatorId);
            await _chatroomRepo.SaveChangesAsync();
        }
    }
}