using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chat.Server.Domains.Entities;
using Chat.Server.Infrastructure.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.Server
{
    public partial class Server
    {
        public async Task<Chatroom> NewChatroomAsync(IEnumerable<long> peopleIds)
        {
            var peoples = peopleIds.Select(id => _userRepo.GetByIdAsync(id).Result);
            var chatroom = await _chatroomRepo.NewEmptyChatroomAsync("", creatorId: 0);
            foreach (var user in peoples)
                chatroom.AddPeople(user);
            await _chatroomRepo.SaveChangesAsync();
            return chatroom;
        }

        public async Task AddPeopleToChatroom(long chatroomId, long userId)
        {
            var chatroom = await _chatroomRepo.GetByIdAsync(chatroomId);
            var people = await _userRepo.GetByIdAsync(userId);
            chatroom.AddPeople(people);
            await _chatroomRepo.SaveChangesAsync();
            await _userRepo.SaveChangesAsync();
        }

        public async Task MakeFriends(long user1Id, long user2Id)
        {
            var user1 = await _userRepo.GetByIdAsync(user1Id);
            var user2 = await _userRepo.GetByIdAsync(user2Id);
            user1.MakeFriendsWith(user2);
            await _userRepo.SaveChangesAsync();
        }

        public void ClearDatabase()
        {
            var _context = _provider.GetRequiredService<ServerDbContext>();
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
            _context.Database.Migrate();
        }

        public async Task RemovePeopleFromChatroom(long chatroomId, long userId)
        {
            var chatroom = await _chatroomRepo.GetByIdAsync(chatroomId);
            var people = await _userRepo.GetByIdAsync(userId);
            chatroom.RemovePeople(people);
            await _chatroomRepo.SaveChangesAsync();
        }
    }
}