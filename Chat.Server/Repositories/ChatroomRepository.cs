using System;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Chat.Server.Domains;

namespace Chat.Server.Repositories
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
