using Building.OwnersAPI.Core.Configurations;
using Building.OwnersAPI.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Building.OwnersAPI.Core.Context
{
    public class OwnerContext : DbContext
    {
        public OwnerContext(DbContextOptions<OwnerContext> options)
            : base(options)
        {
        }

        public DbSet<Owner> Owners { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new OwnerConfiguration());
        }
    }
}
