using API.Models;

namespace API.Interfaces
{
    public interface IUnitOfWork
    {
        IRepository<Property> Properies { get; }
    }
}
