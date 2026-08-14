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

        // The constructor assigns the id, so EF must not try to supply one.
        builder.Property(h => h.Id)
            .ValueGeneratedNever();

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

                    join.HasData(
                        from hallId in SeedIds.AllHalls
                        from amenityId in SeedIds.AllAmenities
                        select new { HallId = hallId, AmenityId = amenityId });
                });

        builder.HasData(
            new { Id = SeedIds.HallA, Name = "Hall A", Capacity = 50, BaseHourlyRate = 2000m },
            new { Id = SeedIds.HallB, Name = "Hall B", Capacity = 100, BaseHourlyRate = 3500m },
            new { Id = SeedIds.HallC, Name = "Hall C", Capacity = 30, BaseHourlyRate = 1500m });
    }
}
