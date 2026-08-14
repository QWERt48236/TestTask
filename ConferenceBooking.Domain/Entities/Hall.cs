namespace ConferenceBooking.Domain.Entities;

public class Hall
{
    // Required by EF Core.
    private Hall()
    {
        Name = string.Empty;
    }

    public Hall(string name, int capacity, decimal baseHourlyRate)
    {
        Id = Guid.NewGuid();
        Name = EnsureValidName(name);
        Capacity = EnsureValidCapacity(capacity);
        BaseHourlyRate = EnsureValidRate(baseHourlyRate);
    }

    public Guid Id { get; private set; }

    public string Name { get; private set; }

    public int Capacity { get; private set; }

    // Before time-band discounts and surcharges.
    public decimal BaseHourlyRate { get; private set; }

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
