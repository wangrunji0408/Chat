using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chat.Core.Models;
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
            if(!peopleIds.All(id => _userRepo.ContainsIdAsync(id).Result))
                throw new ArgumentException("User not exist.");
            var chatroom = await _chatroomRepo.NewEmptyChatroomAsync(name, OperatorId);
            foreach (var user in peopleIds)
                chatroom.AddPeople(user, 0);
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
    }
}