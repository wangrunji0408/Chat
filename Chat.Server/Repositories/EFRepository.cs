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
        where TEntity: Domains.DomainBase
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
			_logger = provider.GetService<ILoggerFactory>()?.CreateLogger<UserRepository>();
		}

        public void Add (TEntity entity)
        {
            _set.Add(entity);
        }

		public void Update(TEntity entity)
		{
			_set.Update(entity);
		}

		public void Remove(TEntity entity)
		{
            _set.Remove(entity);
		}

        public virtual async Task<TEntity> FindByIdAsync (long id)
        {
            var entity = await _set.FindAsync(id);
            entity?.SetServices(_provider);
            return entity;
        }

        public async Task<TEntity> GetAsyncById (long id)
        {
            return await FindByIdAsync(id)
                ?? throw new KeyNotFoundException($"{typeof(TEntity).Name} {id} does not exist.");
        }

		public async Task<bool> ContainsIdAsync(long id)
		{
            return await _set.CountAsync(e => e.Id == id) > 0;
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
            return _set.ToAsyncEnumerable();
        }

        public IQueryable Query ()
        {
            return _set;
        }
    }
}
