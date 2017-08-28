using System;
using Microsoft.EntityFrameworkCore;
using Chat.Server.Domains;

namespace Chat.Server
{
    public class ServerDbContext: DbContext
    {
        public DbSet<User> Users { get; set; }

		public ServerDbContext(DbContextOptions<ServerDbContext> options)
            : base(options)
        {

        }
    }
}
