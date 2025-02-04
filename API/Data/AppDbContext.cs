using API.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<PropertyType> PropertyTypes { get; set; }
        //public virtual DbSet<Image> Images { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //builder.Entity<Image>().OwnsOne(i => i.Property);
                

            builder.Entity<Category>().HasData(
                    new PropertyType { Id = 1, Name = "Residential" },
                    new PropertyType { Id = 2, Name = "Commercial" },
                    new PropertyType { Id = 3, Name = "Land" },
                    new PropertyType { Id = 4, Name = "Luxury" },
                    new PropertyType { Id = 5, Name = "Industrial" },
                    new PropertyType { Id = 6, Name = "Agricultural" }
                );

            builder.Entity<PropertyType>().HasData(
                    new PropertyType { Id = 1, Name = "Apartment", CategoryId=1},
                    new PropertyType { Id = 2, Name = "{Villa}", CategoryId = 1 },
                    new PropertyType { Id = 3, Name = "Palace", CategoryId = 1 },
                    new PropertyType { Id = 4, Name = "House", CategoryId = 1 },
                    new PropertyType { Id = 5, Name = "Company" , CategoryId=2},
                    new PropertyType { Id = 6, Name = "Supermarket", CategoryId = 2 },
                    new PropertyType { Id = 7, Name = "Hotel", CategoryId = 2 },
                    new PropertyType { Id = 8, Name = "Showroom", CategoryId = 2 },
                    new PropertyType { Id = 9, Name = "Office", CategoryId = 2 },
                    new PropertyType { Id = 10, Name = "Residential Land", CategoryId = 3},
                    new PropertyType { Id = 11, Name = "Commercial Land", CategoryId = 3 },
                    new PropertyType { Id = 12, Name = "Industrial Land", CategoryId = 3 },
                    new PropertyType { Id = 13, Name = "Agricultural Land", CategoryId = 3 },
                    new PropertyType { Id = 14, Name = "Mansions", CategoryId = 4},
                    new PropertyType { Id = 15, Name = "Beachfront Properties", CategoryId = 4 },
                    new PropertyType { Id = 16, Name = "Gated Communities", CategoryId = 4 },
                    new PropertyType { Id = 17, Name = "Factories", CategoryId = 5},
                    new PropertyType { Id = 18, Name = "Warehouses", CategoryId = 5 },
                    new PropertyType { Id = 19, Name = "Cold Storage", CategoryId = 5 },
                    new PropertyType { Id = 20, Name = "Distribution Centers", CategoryId = 5 },
                    new PropertyType { Id = 21, Name = "Farms", CategoryId = 6 },
                    new PropertyType { Id = 22, Name = "Ranches", CategoryId = 6 },
                    new PropertyType { Id = 23, Name = "Orchards", CategoryId = 6 }
                );

        }
    }
}
