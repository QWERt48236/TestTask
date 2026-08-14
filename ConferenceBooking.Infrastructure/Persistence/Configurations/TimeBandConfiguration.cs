using ConferenceBooking.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConferenceBooking.Infrastructure.Persistence.Configurations;

public class TimeBandConfiguration : IEntityTypeConfiguration<TimeBand>
{
    public void Configure(EntityTypeBuilder<TimeBand> builder)
    {
        builder.ToTable("TimeBands");

        builder.HasKey(b => b.Id);

        // The constructor assigns the id, so EF must not try to supply one.
        builder.Property(b => b.Id)
            .ValueGeneratedNever();

        builder.Property(b => b.Name)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(b => b.Name)
            .IsUnique();

        // Ties would make band selection depend on row order.
        builder.HasIndex(b => b.Priority)
            .IsUnique();

        // Four decimals so a fine-grained modifier like 0.0725 survives.
        builder.Property(b => b.Modifier)
            .HasPrecision(5, 4);

        builder.HasData(
            new { Id = SeedIds.PeakBand, Name = "Peak", Start = new TimeOnly(12, 0), End = new TimeOnly(14, 0), Modifier = 0.15m, Priority = 1 },
            new { Id = SeedIds.EveningBand, Name = "Evening", Start = new TimeOnly(18, 0), End = new TimeOnly(23, 0), Modifier = -0.20m, Priority = 2 },
            new { Id = SeedIds.MorningBand, Name = "Morning", Start = new TimeOnly(6, 0), End = new TimeOnly(9, 0), Modifier = -0.10m, Priority = 3 },
            new { Id = SeedIds.StandardBand, Name = "Standard", Start = new TimeOnly(9, 0), End = new TimeOnly(18, 0), Modifier = 0.00m, Priority = 4 });
    }
}
