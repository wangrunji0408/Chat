using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Chat.Server.Domains;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Chat.Server.Repositories
{
    public class UserRepository: EFRepository<User>
    {
        public UserRepository(IServiceProvider provider)
            : base(provider)
        {
        }

        public override async Task<User> FindByIdAsync(long id)
        {
			var entity = await _set.Include(c => c.UserChatrooms)
								   .FirstOrDefaultAsync(c => c.Id == id);
			entity?.SetServices(_provider);
			return entity;
        }

        public async Task<bool> ContainsUsernameAsync (string username)
        {
            return await _set.CountAsync(u => u.Username == username) > 0;
        }

		public Task<User> FindByUsername(string username)
		{
            return _set.FirstOrDefaultAsync(u => u.Username == username);
		}
    }
}
