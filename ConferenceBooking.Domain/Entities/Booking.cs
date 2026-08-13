namespace ConferenceBooking.Domain.Entities;

/// <summary>
/// A confirmed reservation of a hall for a period of time, with the amenities
/// chosen for it and the price that was charged.
/// </summary>
public class Booking
{
    /// <summary>Required by EF Core to materialise the entity.</summary>
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

    public int Id { get; private set; }

    public int HallId { get; private set; }

    public Hall Hall { get; private set; } = null!;

    /// <summary>Local wall-clock start of the reservation.</summary>
    public DateTime Start { get; private set; }

    public TimeSpan Duration { get; private set; }

    /// <summary>Exclusive end of the reservation, so back-to-back bookings do not overlap.</summary>
    public DateTime End => Start + Duration;

    /// <summary>Amenities chosen for this booking.</summary>
    public ICollection<Amenity> Amenities { get; private set; } = [];

    /// <summary>Hall rental only, after time-band discounts and surcharges.</summary>
    public decimal BaseAmount { get; private set; }

    public decimal AmenitiesAmount { get; private set; }

    /// <summary>
    /// Stored rather than recalculated on read: a later change to the hall's rate
    /// must not rewrite what this booking cost.
    /// </summary>
    public decimal TotalAmount => BaseAmount + AmenitiesAmount;

    /// <summary>Audit stamp, in UTC — unlike <see cref="Start"/>, which is venue-local.</summary>
    public DateTime CreatedAtUtc { get; private set; }

    private static TimeSpan EnsureValidDuration(TimeSpan duration) =>
        duration <= TimeSpan.Zero
            ? throw new ArgumentOutOfRangeException(nameof(duration), duration, "Booking duration must be greater than zero.")
            : duration;

    private static decimal EnsureNotNegative(decimal amount, string paramName) =>
        amount < 0
            ? throw new ArgumentOutOfRangeException(paramName, amount, "Amount cannot be negative.")
            : amount;
}
