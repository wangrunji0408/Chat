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
            return await _chatroomService.NewChatroomAsync(peopleIds, "", 0);
        }

        public async Task AddPeopleToChatroom(long chatroomId, long userId)
        {
            await _chatroomService.AddPeoplesToChatroom(chatroomId, new long[]{userId}, 0);
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
            await _chatroomService.RemovePeoplesFromChatroom(chatroomId, new long[]{userId}, 0);
        }
        
        public async Task DismissChatroomAsync(long chatroomId)
        {
            await _chatroomService.DismissChatroom(chatroomId, 0);
        }
    }
}