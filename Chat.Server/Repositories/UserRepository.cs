using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Chat.Server.Domains;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Chat.Server.Repositories
{
    public class UserRepository: EFRepository<User>
    {
        public UserRepository(IServiceProvider provider)
            : base(provider)
        {
        }

        public override IQueryable<User> Query()
        {
            return base.Query().Include(u => u.UserChatrooms);
        }

        public async Task<bool> ContainsUsernameAsync (string username)
        {
            return await _set.CountAsync(u => u.Username == username) > 0;
        }

        public async Task<User> FindByUsernameAsync(string username)
		{
            var id = await _set.Where(u => u.Username == username)
                               .Select(u => u.Id)
                               .FirstOrDefaultAsync();
            return await FindByIdAsync(id);
		}
    }
}
