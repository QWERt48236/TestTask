using ConferenceBooking.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConferenceBooking.Infrastructure.Persistence.Configurations;

public class AmenityConfiguration : IEntityTypeConfiguration<Amenity>
{
    public void Configure(EntityTypeBuilder<Amenity> builder)
    {
        builder.ToTable("Amenities");

        builder.HasKey(a => a.Id);

        // The constructor assigns the id, so EF must not try to supply one.
        builder.Property(a => a.Id)
            .ValueGeneratedNever();

        builder.Property(a => a.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasIndex(a => a.Name)
            .IsUnique();

        // SQL Server defaults decimal to (18,0), silently truncating every price.
        builder.Property(a => a.Price)
            .HasPrecision(18, 2);

        builder.HasData(
            new { Id = SeedIds.Projector, Name = "Projector", Price = 500m },
            new { Id = SeedIds.WiFi, Name = "Wi-Fi", Price = 300m },
            new { Id = SeedIds.Sound, Name = "Sound", Price = 700m });
    }
}
