using System;
using Microsoft.EntityFrameworkCore;
using Chat.Server.Domains;
using Chat.Core.Models;

namespace Chat.Server
{
    public class ServerDbContext: DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<ChatMessage> Messages { get; set; }

		public ServerDbContext(DbContextOptions<ServerDbContext> options)
            : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ChatMessage>()
                        .OwnsOne(m => m.Content);
        }
    }
}
