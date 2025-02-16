using API.Data;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace API.Services
{
    public class Repository<T>(AppDbContext context) : IRepository<T> where T : class
    {
        public T Create(T entity)
        {
            context.Set<T>().Add(entity);
            return entity;
        }

        public async Task<T> FindAsync(Expression<Func<T, bool>> predicate, params string[]? eagers)
        {
            IQueryable<T> query = context.Set<T>();
            if(eagers is not null && eagers.Length > 0)
            {
                foreach(var eager in eagers)
                     query = query.Include(eager);
            }

           return await query.SingleOrDefaultAsync(predicate);
        }

        public async Task<IReadOnlyList<T>> GetAllAsync(params string[]? eagers)
        {
            IQueryable<T> values = context.Set<T>();
            if(eagers is not null && eagers.Length > 0)
            {
                foreach(var eager in eagers)
                {
                    values = values.Include(eager);
                }
            }
            return await values.ToListAsync();
        }

        public void DeleteAsync(T entity)
        {
            context.Set<T>().Remove(entity);
        }

        public void Update(T entity)
        {
            context.Set<T>().Update(entity);
        }
    }
}
