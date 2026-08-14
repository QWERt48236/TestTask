using ConferenceBooking.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConferenceBooking.Infrastructure.Persistence.Configurations;

public class BookingConfiguration : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        builder.ToTable("Bookings");

        builder.HasKey(b => b.Id);

        // The constructor assigns the id, so EF must not try to supply one.
        builder.Property(b => b.Id)
            .ValueGeneratedNever();

        builder.Property(b => b.Start)
            .IsRequired();

        builder.Property(b => b.Duration)
            .IsRequired();

        builder.Property(b => b.BaseAmount)
            .HasPrecision(18, 2);

        builder.Property(b => b.AmenitiesAmount)
            .HasPrecision(18, 2);

        builder.Property(b => b.CreatedAtUtc)
            .IsRequired();

        // Computed, with no backing field: EF fails at model build without this.
        builder.Ignore(b => b.End);
        builder.Ignore(b => b.TotalAmount);

        // Restrict, not the default Cascade: deleting a hall must not erase its bookings.
        builder.HasOne(b => b.Hall)
            .WithMany()
            .HasForeignKey(b => b.HallId)
            .OnDelete(DeleteBehavior.Restrict);

        // Availability checks filter on exactly this pair.
        builder.HasIndex(b => new { b.HallId, b.Start });


        builder.HasMany(b => b.Amenities)
            .WithMany()
            .UsingEntity(
                "BookingAmenities",
                right => right.HasOne(typeof(Amenity))
                    .WithMany()
                    .HasForeignKey("AmenityId")
                    // A booked amenity cannot be deleted out from under the booking.
                    .OnDelete(DeleteBehavior.Restrict),
                left => left.HasOne(typeof(Booking))
                    .WithMany()
                    .HasForeignKey("BookingId")
                    .OnDelete(DeleteBehavior.Cascade),
                join => join.HasKey("BookingId", "AmenityId"));
    }
}
