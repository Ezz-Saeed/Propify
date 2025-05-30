﻿using API.Data;
using API.Interfaces;
using API.Models;

namespace API.Services
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext context;

        public UnitOfWork(AppDbContext context)
        {
            Properies = new Repository<Property>(context);
            Types = new Repository<PropertyType>(context);
            Categories = new Repository<Category>(context);
            this.context = context;
        }
        public IRepository<Property> Properies { get; private set; }

        public IRepository<PropertyType> Types { get; private set; }
        public IRepository<Category> Categories { get; private set; }

        public async Task<bool> Dispose()
        {
            return await context.SaveChangesAsync() > 0;
        }
    }
}
