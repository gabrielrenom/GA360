using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using GA360.DAL.Infrastructure.Interfaces;
using GA360.DAL.Entities.BaseEntities;
using GA360.DAL.Infrastructure.Contexts;
using System.Reflection.Metadata.Ecma335;

namespace GA360.DAL.Infrastructure.Repositories
{
    public abstract class Repository<T> : IRepository<T>
        where T : class, IModel
    {
        public readonly CRMDbContext _dbContext;

        public CRMDbContext Context { get => _dbContext; }

        public Repository(CRMDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        protected CRMDbContext GetDbContext() { return _dbContext; }
        public T Get(int key) => _dbContext.Set<T>().Where(p => p.Id == key).Single();
        public async Task<T> Get<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            return await _dbContext.Set<T>().FirstOrDefaultAsync(predicate);
        }
        public void Add(T entity) => _dbContext.Add<T>(entity);
        public T Find(Expression<Func<T, bool>> expression) => _dbContext.Set<T>().Where(expression).FirstOrDefault();
        public async Task<List<T>> GetAll() => await _dbContext.Set<T>().ToListAsync();
        public async Task<List<T>> GetAll(params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbContext.Set<T>();

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return await query.ToListAsync();
        }

        public async Task<List<T>> GetAll(params (Expression<Func<T, object>> include, Expression<Func<object, object>>[] thenIncludes)[] includes)
        {
            IQueryable<T> query = _dbContext.Set<T>();

            foreach (var include in includes)
            {
                var initialInclude = query.Include(include.include);
                foreach (var thenInclude in include.thenIncludes)
                {
                    initialInclude = initialInclude.ThenInclude(thenInclude);
                }
                query = (IQueryable<T>)initialInclude;
            }

            return await query.ToListAsync();
        }

        public IEnumerable<T> FindAll(Expression<Func<T, bool>> expression) => _dbContext.Set<T>().Where(expression);

        public void Update(T entity) => _dbContext.Update<T>(entity);

        public void Delete(T entity) => _dbContext.Remove<T>(entity);

        public int Count(Expression<Func<T, bool>> expression) => _dbContext.Set<T>().Where(expression).Count();

        public void SaveChanges() => _dbContext.SaveChanges();
        public async Task SaveChangesAsync() => await _dbContext.SaveChangesAsync();
    }

}
