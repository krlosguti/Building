using Building.OwnersAPI.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Building.OwnersAPI.Core.Configurations
{
    public class OwnerConfiguration : IEntityTypeConfiguration<Owner>
    {
        /// <summary>
        /// Configurate fields of the owner model
        /// This information is used when the database is created
        /// </summary>
        /// <param name="entity"></param>
        public void Configure(EntityTypeBuilder<Owner> entity)
        {
            entity.HasKey(e => e.IdOwner);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Address).HasMaxLength(100);
            entity.Property(e => e.Photo).HasMaxLength(200);
        }
    }
}
