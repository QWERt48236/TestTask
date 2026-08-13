namespace ConferenceBooking.Domain.Entities;

/// <summary>
/// A conference hall available for rent, with a capacity, an hourly rate,
/// and the set of amenities it offers.
/// </summary>
public class Hall
{
    /// <summary>Required by EF Core to materialise the entity.</summary>
    private Hall()
    {
        Name = string.Empty;
    }

    public Hall(string name, int capacity, decimal baseHourlyRate)
    {
        Name = EnsureValidName(name);
        Capacity = EnsureValidCapacity(capacity);
        BaseHourlyRate = EnsureValidRate(baseHourlyRate);
    }

    public int Id { get; private set; }

    public string Name { get; private set; }

    /// <summary>Maximum number of people the hall seats.</summary>
    public int Capacity { get; private set; }

    /// <summary>Rate before any time-band discount or surcharge is applied.</summary>
    public decimal BaseHourlyRate { get; private set; }

    /// <summary>Amenities this hall offers.</summary>
    public ICollection<Amenity> Amenities { get; private set; } = [];

    public void Update(string name, int capacity, decimal baseHourlyRate)
    {
        Name = EnsureValidName(name);
        Capacity = EnsureValidCapacity(capacity);
        BaseHourlyRate = EnsureValidRate(baseHourlyRate);
    }

    private static string EnsureValidName(string name) =>
        string.IsNullOrWhiteSpace(name)
            ? throw new ArgumentException("Hall name is required.", nameof(name))
            : name.Trim();

    private static int EnsureValidCapacity(int capacity) =>
        capacity <= 0
            ? throw new ArgumentOutOfRangeException(nameof(capacity), capacity, "Hall capacity must be greater than zero.")
            : capacity;

    private static decimal EnsureValidRate(decimal baseHourlyRate) =>
        baseHourlyRate <= 0
            ? throw new ArgumentOutOfRangeException(nameof(baseHourlyRate), baseHourlyRate, "Hourly rate must be greater than zero.")
            : baseHourlyRate;
}
