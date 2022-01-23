using Building.PropertyAPI.Core.Configurations;
using Building.PropertyAPI.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Building.PropertyAPI.Core.Context
{
    /// <summary>
    /// context to connect with property database.  It has two tables property and propertyImage
    /// </summary>
    public class PropertyContext : DbContext
    {
        public PropertyContext(DbContextOptions<PropertyContext> options)
            : base(options)
        {
        }

        public DbSet<Property> Property { get; set; }
        public DbSet<PropertyImage> PropertyImage { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new PropertyConfiguration());
            modelBuilder.ApplyConfiguration(new PropertyImageConfiguration());
        }
    }
}
