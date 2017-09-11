using System;
using System.Linq;
using System.Threading.Tasks;
using Chat.Server.Domains.Entities;
using Chat.Server.Domains.Events.Chatroom;
using Chat.Server.Domains.Repositories;
using Chat.Server.Infrastructure.EventBus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.Server.Infrastructure.EntityFramework
{
    public class ChatroomRepository : EFRepository<Chatroom>, IChatroomRepository
    {
        public ChatroomRepository(IServiceProvider provider) : base(provider)
        {
        }

        public override IQueryable<Chatroom> Query()
        {
            return base.Query().Include(c => c.UserChatrooms);
        }

        public async Task<Chatroom> NewEmptyChatroomAsync(string name, long creatorId)
        {
            if(name == null)
                throw new ArgumentNullException(nameof(name));
            var chatroom = new Chatroom(name, creatorId);
            chatroom.SetServices(_provider);
            Add(chatroom);
            await SaveChangesAsync();
            _provider.GetRequiredService<IEventBus>().Publish(
                new NewChatroomEvent(chatroom.Id, creatorId));
            return chatroom;
        }
        
        public async Task<Chatroom> FindP2PChatroomAsync(long user1Id, long user2Id)
        {
            if (user1Id > user2Id)
            {
                var t = user1Id;
                user1Id = user2Id;
                user2Id = t;
            }

            return await _set.FirstOrDefaultAsync(
                c => c.P2PUser1Id == user1Id && c.P2PUser2Id == user2Id);
        }
    }
}
