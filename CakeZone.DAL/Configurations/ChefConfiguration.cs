using CakeZone.CORE.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CakeZone.DAL.Configurations;

public class ChefConfiguration : IEntityTypeConfiguration<Chef>
{
    public void Configure(EntityTypeBuilder<Chef> builder)
    {
        builder.Property(x => x.FullName)
            .IsRequired()
            .HasMaxLength(64);

        builder.Property(x => x.ImageUrl)
            .IsRequired();


        builder.HasOne(x => x.Designation)
            .WithMany(x => x.Chefs)
            .HasForeignKey(x => x.DesignationId);
    }
}
