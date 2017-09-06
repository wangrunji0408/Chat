using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chat.Server.Domains;
using Microsoft.EntityFrameworkCore;

namespace Chat.Server
{
    public partial class Server
    {
        public async Task<Chatroom> NewChatroomAsync(IEnumerable<long> peopleIds)
        {
            var peoples = peopleIds.Select(id => _userRepo.GetByIdAsync(id).Result);
            var chatroom = new Chatroom();
            foreach (var user in peoples)
                chatroom.NewPeople(user);
            _chatroomRepo.Add(chatroom);
            await _chatroomRepo.SaveChangesAsync();
            return chatroom;
        }

        public async Task AddPeopleToChatroom(long chatroomId, long userId)
        {
            var chatroom = await _chatroomRepo.GetByIdAsync(chatroomId);
            var people = await _userRepo.GetByIdAsync(userId);
            chatroom.NewPeople(people);
            await _chatroomRepo.SaveChangesAsync();
        }

        public async Task MakeFriends(long user1Id, long user2Id)
        {
            var user1 = await _userRepo.GetByIdAsync(user1Id);
            var user2 = await _userRepo.GetByIdAsync(user2Id);
            UserService.MakeFriends(user1, user2);
            await _userRepo.SaveChangesAsync();
        }

        public void ClearDatabase()
        {
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
            _context.Database.Migrate();
        }
    }
}