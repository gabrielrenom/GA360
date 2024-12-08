using GA360.DAL.Entities.BaseEntities;
using GA360.DAL.Infrastructure.Contexts;
using System.Linq.Expressions;

namespace GA360.DAL.Infrastructure.Interfaces;
public interface IRepository<T> where T : IModel
{
    CRMDbContext Context{ get;}
    T Get(int key);
    Task<T> GetAsync(int key);
    Task<List<T>> GetAll();
    Task<T> Get<T>(Expression<Func<T, bool>> predicate) where T : class;
    Task<List<T>> GetAll(params Expression<Func<T, object>>[] includes);
    Task<List<T>> GetAll(params (Expression<Func<T, object>> include, Expression<Func<object, object>>[] thenIncludes)[] includes);
    void Add(T entity);
    Task<T> AddAsync(T entity);
    T Find(Expression<Func<T, bool>> expression);
    IEnumerable<T> FindAll(Expression<Func<T, bool>> expression);
    void Update(T entity);
    Task<T> UpdateAsync(T entity);
    void Delete(T entity);
    void SaveChanges();
    Task SaveChangesAsync();
    int Count(Expression<Func<T, bool>> expression);
}
