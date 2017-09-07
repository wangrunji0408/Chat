using System.Threading.Tasks;
using Chat.Core.Models;
using Chat.Server.Domains.Entities;
using Microsoft.EntityFrameworkCore;

namespace Chat.Server.Infrastructure.EntityFramework
{
    public class ServerDbContext: DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Chatroom> Chatrooms { get; set; }
        public DbSet<ChatMessage> Messages { get; set; }

		public ServerDbContext(DbContextOptions<ServerDbContext> options)
            : base(options)
        {
//            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ChatMessage>()
                .Ignore(m => m.Content);

            modelBuilder.Entity<UserChatroom>()
                        .HasKey(uc => new { uc.UserId, uc.ChatroomId });
            modelBuilder.Entity<UserChatroom>()
                        .HasOne(uc => uc.User)
                        .WithMany(u => u.UserChatrooms)
                        .HasForeignKey(uc => uc.UserId)
                        .IsRequired();
			modelBuilder.Entity<UserChatroom>()
						.HasOne(uc => uc.Chatroom)
                        .WithMany(c => c.UserChatrooms)
                        .HasForeignKey(uc => uc.ChatroomId)
                        .IsRequired();

            modelBuilder
                .Entity<UserRelationship>()
                .HasKey(r => new {r.FromUserId, r.ToUserId});
            
            modelBuilder
                .Entity<UserRelationship>()
                .HasOne(r => r.FromUser)
                .WithMany(u => u.UserRelationships)
                .HasForeignKey(r => r.FromUserId)
                .IsRequired();
        }
    }

    public static class DbSetExtension
    {
        public static async Task<bool> ContainsIdAsync<TDomain> (this DbSet<TDomain> dbSet, long id)
            where TDomain: DomainBase
        {
            return await dbSet.CountAsync(e => e.Id == id) > 0;
        }
    }

    // http://www.cnblogs.com/Kation/p/efcore_entity_init_inject.html
   // public class JJ: EntityMaterializerSource
   // {
   //     public override Expression CreateMaterializeExpression(IEntityType entityType, Expression valueBufferExpression, int[] indexMap = null)
   //     {
			//BlockExpression expression = (BlockExpression)base.CreateMaterializeExpression(entityType, valueBufferExpression, indexMap);

			//if (typeof(IEntity).IsAssignableFrom(entityType.ClrType))
			//{
			//	var property = Expression.Property(expression.Variables[0], typeof(int).GetProperty("EntityContext"));
			//	var assign = Expression.Assign(property, Expression.Constant(null));
   //             var list = expression.Expressions.Concat(new [] { assign });
			//	expression = Expression.Block(expression.Variables, list);
			//}
			//return expression;
    //    }
    //}
}
