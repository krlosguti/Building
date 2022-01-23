using Building.PropertyAPI.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Building.PropertyAPI.Core.Configurations
{
    public class PropertyImageConfiguration : IEntityTypeConfiguration<PropertyImage>
    {
        //Validates information of a property image
        //it is used when the database is created
        public void Configure(EntityTypeBuilder<PropertyImage> entity)
        {
            entity.HasKey(e => e.IdPropertyImage);
            entity.Property(e => e.File).HasMaxLength(150);
        }
    }
}
