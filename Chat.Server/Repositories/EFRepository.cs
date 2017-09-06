using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Chat.Server.Repositories
{
    public class EFRepository<TEntity>
        where TEntity:class
    {
        protected readonly DbSet<TEntity> _set;
        protected readonly DbContext _context;
		protected readonly ILogger _logger;
        protected readonly IServiceProvider _provider;

		public EFRepository(IServiceProvider provider)
		{
            _provider = provider;
            _context = provider.GetRequiredService<ServerDbContext>();
            _set = _context.Set<TEntity>();
			_logger = provider.GetService<ILoggerFactory>()?
                              .CreateLogger<EFRepository<TEntity>>();
		}

        public void Add (TEntity entity)
        {
            _set.Add(entity);
        }

		public void Update(TEntity entity)
		{
		    // Entity has been tracked.
//			_set.Update(entity);
		}

		public void Remove(TEntity entity)
		{
            _set.Remove(entity);
		}

        /// <summary>
        /// 根据Id找到聚合根。自动加载关联Entity，并进行依赖注入。
        /// </summary>
        /// <param name="id">Identifier.</param>
        /// <returns>聚合对象，可能为空。</returns>
        public virtual async Task<TEntity> FindByIdAsync (long id)
        {
            var entity = await _set.FindAsync(id);
            await LoadRelationsAsync(entity);
            (entity as Domains.DomainBase)?.SetServices(_provider);
            return entity;
        }

        public async Task<TEntity> GetByIdAsync (long id)
        {
            return await FindByIdAsync(id)
                ?? throw new KeyNotFoundException($"{typeof(TEntity).Name} {id} does not exist.");
        }

		public async Task<bool> ContainsIdAsync(long id)
		{
            return await _set.CountAsync(e => EF.Property<long>(e, "Id") == id) > 0;
		}

        public Task<int> SaveChangesAsync ()
        {
            return _context.SaveChangesAsync();
        }

        public int SaveChanges ()
        {
            return _context.SaveChanges();
        }

        public IAsyncEnumerable<TEntity> GetAllAsync ()
        {
            return Query().ToAsyncEnumerable();
        }

        /// <summary>
        /// Override this with _set.Include().
        /// 只可用于查询端，不可用于命令端，因为会返回新的对象。
        /// </summary>
        /// <returns>The query.</returns>
        public virtual IQueryable<TEntity> Query ()
        {
            return _set.AsNoTracking();
        }

        /// <summary>
        /// Load all relations by default. It is called in Find/GetById.
        /// </summary>
        protected virtual async Task LoadRelationsAsync (TEntity entity)
        {
            if(entity == null)    return;
			foreach (var coll in _context.Entry(entity).Collections)
				await coll.LoadAsync();
        }
    }
}
