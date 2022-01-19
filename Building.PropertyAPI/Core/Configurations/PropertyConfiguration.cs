using Building.PropertyAPI.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Building.PropertyAPI.Core.Configurations
{
    public class PropertyConfiguration : IEntityTypeConfiguration<Property>
    {
        public void Configure(EntityTypeBuilder<Property> entity)
        {
            entity.HasKey(e => e.IdProperty);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Address).HasMaxLength(100);
            entity.Property(e => e.CodeInternal).HasMaxLength(30);
        }
    }
}
