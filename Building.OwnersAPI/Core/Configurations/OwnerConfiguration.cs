using Building.OwnersAPI.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Building.OwnersAPI.Core.Configurations
{
    public class OwnerConfiguration : IEntityTypeConfiguration<Owner>
    {
        public void Configure(EntityTypeBuilder<Owner> entity)
        {
            entity.HasKey(e => e.IdOwner);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Address).HasMaxLength(100);
            entity.Property(e => e.Photo).HasMaxLength(200);
        }
    }
}
