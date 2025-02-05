using API.Data;
using API.Interfaces;
using API.Models;

namespace API.Services
{
    public class UnitOfWork : IUnitOfWork
    {
        public UnitOfWork(AppDbContext context)
        {
            Properies = new Repository<Property>(context);
        }
        public IRepository<Property> Properies { get; private set; }
    }
}
