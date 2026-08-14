namespace ConferenceBooking.Domain.Entities;

public class Booking
{
    // Anything longer is a multi-day event, priced differently. Also keeps Duration
    // inside SQL Server's time column, which cannot hold 24 hours or more.
    public static readonly TimeSpan MaxDuration = TimeSpan.FromHours(12);

    // Required by EF Core.
    private Booking()
    {
    }

    public Booking(
        Hall hall,
        DateTime start,
        TimeSpan duration,
        IEnumerable<Amenity> amenities,
        decimal baseAmount,
        decimal amenitiesAmount)
    {
        ArgumentNullException.ThrowIfNull(hall);
        ArgumentNullException.ThrowIfNull(amenities);

        Id = Guid.NewGuid();
        Hall = hall;
        HallId = hall.Id;
        Start = start;
        Duration = EnsureValidDuration(duration);
        BaseAmount = EnsureNotNegative(baseAmount, nameof(baseAmount));
        AmenitiesAmount = EnsureNotNegative(amenitiesAmount, nameof(amenitiesAmount));
        foreach (var amenity in amenities)
        {
            Amenities.Add(amenity);
        }

        CreatedAtUtc = DateTime.UtcNow;
    }

    public Guid Id { get; private set; }

    public Guid HallId { get; private set; }

    public Hall Hall { get; private set; } = null!;

    // Venue-local wall clock.
    public DateTime Start { get; private set; }

    public TimeSpan Duration { get; private set; }

    // Exclusive, so back-to-back bookings do not overlap.
    public DateTime End => Start + Duration;

    public ICollection<Amenity> Amenities { get; private set; } = [];

    // Stored, not recalculated: a later rate change must not rewrite past bookings.
    public decimal BaseAmount { get; private set; }

    public decimal AmenitiesAmount { get; private set; }

    public decimal TotalAmount => BaseAmount + AmenitiesAmount;

    // UTC, unlike Start.
    public DateTime CreatedAtUtc { get; private set; }

    private static TimeSpan EnsureValidDuration(TimeSpan duration)
    {
        if (duration <= TimeSpan.Zero)
        {
            throw new ArgumentOutOfRangeException(
                nameof(duration), duration, "Booking duration must be greater than zero.");
        }

        if (duration > MaxDuration)
        {
            throw new ArgumentOutOfRangeException(
                nameof(duration), duration, $"Booking duration cannot exceed {MaxDuration.TotalHours} hours.");
        }

        return duration;
    }

    private static decimal EnsureNotNegative(decimal amount, string paramName) =>
        amount < 0
            ? throw new ArgumentOutOfRangeException(paramName, amount, "Amount cannot be negative.")
            : amount;
}
