using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chat.Core.Models;
using Chat.Server.Domains.Entities;

namespace Chat.Server.Domains.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        void Add (TEntity entity);
        void Update(TEntity entity);
        void Remove(TEntity entity);

        /// <summary>
        /// 根据Id找到聚合根。自动加载关联Entity，并进行依赖注入。
        /// </summary>
        /// <param name="id">Identifier.</param>
        /// <returns>聚合对象，可能为空。</returns>
        Task<TEntity> FindByIdAsync (long id);

        Task<TEntity> GetByIdAsync (long id);
        Task<bool> ContainsIdAsync(long id);
        Task<int> SaveChangesAsync ();
        int SaveChanges ();
        IAsyncEnumerable<TEntity> GetAllAsync ();

        /// <summary>
        /// Override this with _set.Include().
        /// 只可用于查询端，不可用于命令端，因为会返回新的对象。
        /// </summary>
        /// <returns>The query.</returns>
        IQueryable<TEntity> Query ();
    }
    
    public interface IMessageRepository: IRepository<ChatMessage>
    {
    }
    
    public interface IChatroomRepository: IRepository<Chatroom>
    {
        Task<Chatroom> NewEmptyChatroomAsync(string name = "");
        Task<Chatroom> FindP2PChatroomAsync(long user1Id, long user2Id);
    }
    
    public interface IUserRepository: IRepository<User>
    {
        Task<bool> ContainsUsernameAsync (string username);
        Task<User> FindByUsernameAsync(string username);
    }
}