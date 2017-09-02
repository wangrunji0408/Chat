using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Chat.Server.Domains;
using Chat.Core.Models;

namespace Chat.Server
{
    public class ServerDbContext: DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Chatroom> Chatrooms { get; set; }
        public DbSet<ChatMessage> Messages { get; set; }
        public IServiceProvider ServiceProvider { get; set; }

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

            modelBuilder.Entity<UserChatroom>()
                        .HasKey(uc => new { uc.UserId, uc.ChatroomId });
            modelBuilder.Entity<UserChatroom>()
                        .HasOne(uc => uc.User)
                        .WithMany(u => u.UserChatrooms)
                        .HasForeignKey(uc => uc.UserId);
			modelBuilder.Entity<UserChatroom>()
						.HasOne(uc => uc.Chatroom)
                        .WithMany(c => c.UserChatrooms)
                        .HasForeignKey(uc => uc.ChatroomId);
        }

        public override TEntity Find<TEntity>(params object[] keyValues)
        {
            var obj =  base.Find<TEntity>(keyValues);
            var domain = obj as DomainBase;
            if(domain != null)
            {
				domain.SetServices(ServiceProvider);
				domain._context = this;
            }
            return obj;
        }
    }
}
