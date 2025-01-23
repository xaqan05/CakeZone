using CakeZone.CORE.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CakeZone.DAL.Configurations;

internal class DesignationConfiguration : IEntityTypeConfiguration<Designation>
{
    public void Configure(EntityTypeBuilder<Designation> builder)
    {
        builder.HasIndex(x => x.Name).IsUnique();

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(64);

        builder.HasMany(x => x.Chefs)
            .WithOne(x => x.Designation)
            .HasForeignKey(x => x.DesignationId);
    }
}
