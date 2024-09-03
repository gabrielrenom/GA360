using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using GA360.DAL.Infrastructure.Interfaces;
using GA360.DAL.Entities.BaseEntities;

namespace GA360.DAL.Infrastructure.Repositories
{
    public abstract class Repository<T> : IRepository<T>
        where T : class, IModel
    {
        private readonly DbContext _dbContext;
        public Repository(DbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        protected DbContext GetDbContext() { return _dbContext; }
        public T Get(int key) => _dbContext.Set<T>().Where(p => p.Id == key).Single();

        public void Add(T entity) => _dbContext.Add<T>(entity);

        public T Find(Expression<Func<T, bool>> expression) => _dbContext.Set<T>().Where(expression).FirstOrDefault();
        public async Task<List<T>> GetAll() => await _dbContext.Set<T>().ToListAsync();

        public IEnumerable<T> FindAll(Expression<Func<T, bool>> expression) => _dbContext.Set<T>().Where(expression);

        public void Update(T entity) => _dbContext.Update<T>(entity);

        public void Delete(T entity) => _dbContext.Remove<T>(entity);

        public int Count(Expression<Func<T, bool>> expression) => _dbContext.Set<T>().Where(expression).Count();

        public void SaveChanges() => _dbContext.SaveChanges();
        public async Task SaveChangesAsync() => await _dbContext.SaveChangesAsync();
    }

}
