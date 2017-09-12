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
            var ca = await GetChatroomApplication(0, 0).NewChatroomAsync(peopleIds);
            return await _chatroomRepo.GetByIdAsync(ca.ChatroomId);
        }
        
        public async Task MakeFriendsAsync(long user1Id, long user2Id)
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
    }
}