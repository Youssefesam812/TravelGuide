using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Snap.Core.Entities;

public class TopPlaceConfiguration : IEntityTypeConfiguration<TopPlace>
{
    public void Configure(EntityTypeBuilder<TopPlace> builder)
    {
        builder.HasKey(tp => tp.Id);

        builder.Property(tp => tp.Name)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(tp => tp.ImageUrl)
            .HasMaxLength(500);

        builder.Property(tp => tp.Description)
            .HasColumnType("TEXT");

        // Foreign Key Relationship
        builder.HasOne(tp => tp.Governorate)
            .WithMany(g => g.TopPlaces)
            .HasForeignKey(tp => tp.GovernorateId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
