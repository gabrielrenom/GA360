using System.Linq.Expressions;

namespace GA360.DAL.Infrastructure.Interfaces;
public interface IRepository<T> where T : IModel
{
    T Get(Guid key);
    void Add(T entity);
    T Find(Expression<Func<T, bool>> expression);
    IEnumerable<T> FindAll(Expression<Func<T, bool>> expression);
    void Update(T entity);
    void Delete(T entity);
    void SaveChanges();
    int Count(Expression<Func<T, bool>> expression);
}
