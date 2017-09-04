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

        public override async Task<Chatroom> FindByIdAsync(long id)
        {
            var entity = await _set.Include(c => c.UserChatrooms)
                                   .FirstOrDefaultAsync(c => c.Id == id);
			entity?.SetServices(_provider);
			return entity;
        }
    }
}
