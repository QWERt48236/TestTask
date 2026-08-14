using ConferenceBooking.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConferenceBooking.Infrastructure.Persistence.Configurations;

public class HallConfiguration : IEntityTypeConfiguration<Hall>
{
    public void Configure(EntityTypeBuilder<Hall> builder)
    {
        builder.ToTable("Halls");

        builder.HasKey(h => h.Id);

        builder.Property(h => h.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasIndex(h => h.Name)
            .IsUnique();

        builder.Property(h => h.Capacity)
            .IsRequired();

        builder.Property(h => h.BaseHourlyRate)
            .HasPrecision(18, 2);

        // Spelled out because HasData below needs predictable column names.
        builder.HasMany(h => h.Amenities)
            .WithMany()
            .UsingEntity(
                "HallAmenities",
                right => right.HasOne(typeof(Amenity))
                    .WithMany()
                    .HasForeignKey("AmenityId")
                    .OnDelete(DeleteBehavior.Cascade),
                left => left.HasOne(typeof(Hall))
                    .WithMany()
                    .HasForeignKey("HallId")
                    .OnDelete(DeleteBehavior.Cascade),
                join =>
                {
                    join.HasKey("HallId", "AmenityId");

                    // Every hall offers the full catalogue.
                    join.HasData(
                        new { HallId = 1, AmenityId = 1 },
                        new { HallId = 1, AmenityId = 2 },
                        new { HallId = 1, AmenityId = 3 },
                        new { HallId = 2, AmenityId = 1 },
                        new { HallId = 2, AmenityId = 2 },
                        new { HallId = 2, AmenityId = 3 },
                        new { HallId = 3, AmenityId = 1 },
                        new { HallId = 3, AmenityId = 2 },
                        new { HallId = 3, AmenityId = 3 });
                });

        builder.HasData(
            new { Id = 1, Name = "Hall A", Capacity = 50, BaseHourlyRate = 2000m },
            new { Id = 2, Name = "Hall B", Capacity = 100, BaseHourlyRate = 3500m },
            new { Id = 3, Name = "Hall C", Capacity = 30, BaseHourlyRate = 1500m });
    }
}
