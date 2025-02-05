using API.Data;
using API.Interfaces;

namespace API.Services
{
    public class Repository<T>(AppDbContext context) : IRepository<T> where T : class
    {
        public T CreateAsync(T entity)
        {
            context.Set<T>().Add(entity);
            return entity;
        }
    }
}
