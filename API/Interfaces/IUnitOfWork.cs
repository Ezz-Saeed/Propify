using API.Models;

namespace API.Interfaces
{
    public interface IUnitOfWork
    {
        IRepository<Property> Properies { get; }
        IRepository<PropertyType> Types { get; }
        Task<bool>Dispose();
    }
}
