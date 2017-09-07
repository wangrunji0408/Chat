using System;
using System.Linq;
using Chat.Server.Domains.Entities;
using Chat.Server.Infrastructure.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace Chat.Server.Domains.Repositories
{
    public class ChatroomRepository : EFRepository<Chatroom>
    {
        public ChatroomRepository(IServiceProvider provider) : base(provider)
        {
        }

        public override IQueryable<Chatroom> Query()
        {
            return base.Query().Include(c => c.UserChatrooms);
        }
    }
}
