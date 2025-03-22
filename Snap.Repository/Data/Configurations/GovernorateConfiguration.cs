using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Snap.Core.Entities;

public class GovernorateConfiguration : IEntityTypeConfiguration<Governorate>
{
    public void Configure(EntityTypeBuilder<Governorate> builder)
    {
        builder.HasKey(g => g.Id);

        builder.Property(g => g.Name)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(g => g.ImageUrl)
            .HasMaxLength(500);

        builder.Property(g => g.Description)
            .HasColumnType("TEXT");

        // Relationship: One Governorate has many TopPlaces
        builder.HasMany(g => g.TopPlaces)
            .WithOne(tp => tp.Governorate)
            .HasForeignKey(tp => tp.GovernorateId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
