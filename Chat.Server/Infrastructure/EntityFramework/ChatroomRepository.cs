using System;
using System.Linq;
using System.Threading.Tasks;
using Chat.Server.Domains.Entities;
using Chat.Server.Domains.Repositories;
using Microsoft.EntityFrameworkCore;

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

        public async Task<Chatroom> NewEmptyChatroom(string name)
        {
            if(name == null)
                throw new ArgumentNullException(nameof(name));
            var chatroom = new Chatroom(name);
            chatroom.SetServices(_provider);
            Add(chatroom);
            await SaveChangesAsync();
            return chatroom;
        }
    }
}
