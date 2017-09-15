using Microsoft.AspNetCore.Identity;

namespace Chat.Server.Infrastructure.Identity
{
    public class ChatIdentityUser: IdentityUser<long>
    {
        public ChatIdentityUser()
        {
        }

        public ChatIdentityUser(string userName) : base(userName)
        {
        }
    }
    
    public class ChatIdentityRole: IdentityRole<long>
    {
        public ChatIdentityRole()
        {
        }

        public ChatIdentityRole(string roleName) : base(roleName)
        {
        }
    }
}