using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TriggeriseBEChallenge.Domain;

namespace TriggeriseBEChallenge.Data
{
    public interface IRepository<TEntity> where TEntity : class
    {
        TEntity FindById(object id);
        Task<TEntity> FindByIdAsync(object id);
        Task AddAsync(TEntity entity);
        Task AddRangeAsync(IEnumerable<TEntity> entities);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> predicate);
        IEnumerable<TEntity> Get(
           Expression<Func<TEntity, bool>> filter = null,
           Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
           List<Expression<Func<TEntity, object>>> includeProperties = null,
           int? page = null,
           int? pageSize = null);

        void SaveChanges();
    }

    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly DbContext _context;
        public Repository(DbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task AddAsync(TEntity entity)
            => await _context.AddAsync(entity);

        public async Task AddRangeAsync(IEnumerable<TEntity> entities)
        {
            await _context.AddRangeAsync(entities);
        }

        public async Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> predicate)
            => await _context.Set<TEntity>()
                .Where(predicate)
                .FirstOrDefaultAsync();


        public TEntity FindById(object id)
            => _context.Find<TEntity>(id);


        public async Task<TEntity> FindByIdAsync(object id)
            => await _context.FindAsync<TEntity>(id);


        public IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            List<Expression<Func<TEntity, object>>> includeProperties = null,
            int? page = null,
            int? pageSize = null)
        {
            var query = _context.Set<TEntity>().AsQueryable();

            if (includeProperties != null)
                includeProperties.ForEach(i => { query.Include(i); });

            if (filter != null)
                query = query.Where(filter);

            if (orderBy != null)
                query = orderBy(query);

            if (page != null && pageSize != null)
                query = query.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);

            return query;
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
            => await _context.Set<TEntity>().ToListAsync();

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
