using System.Linq.Expressions;

namespace API.Interfaces
{
    public interface IRepository<T> where T : class
    {
        T Create(T entity);
        Task<IReadOnlyList<T>> GetAllAsync(params string[]? eagers);
        Task<T> FindAsync(Expression<Func<T,bool>> predicate, params string[] eagers);
        void Update(T entity);
        void DeleteAsync(T entity);
    }
}
