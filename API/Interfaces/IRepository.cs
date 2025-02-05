namespace API.Interfaces
{
    public interface IRepository<T> where T : class
    {
        T Create(T entity);
        Task<IReadOnlyList<T>> GetAllAsync(params string[]? eagers);
    }
}
