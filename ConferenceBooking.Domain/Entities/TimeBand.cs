namespace ConferenceBooking.Domain.Entities;

// A window of the day priced differently from the hall's base rate.
public class TimeBand
{
    // Required by EF Core.
    private TimeBand()
    {
        Name = string.Empty;
    }

    public TimeBand(string name, TimeOnly start, TimeOnly end, decimal modifier, int priority)
    {
        Id = Guid.NewGuid();
        Name = EnsureValidName(name);
        (Start, End) = EnsureValidWindow(start, end);
        Modifier = EnsureValidModifier(modifier);
        Priority = priority;
    }

    public Guid Id { get; private set; }

    public string Name { get; private set; }

    public TimeOnly Start { get; private set; }

    // Exclusive, so a band ending at 12:00 does not own 12:00.
    public TimeOnly End { get; private set; }

    // 0.15 is a 15% surcharge, -0.20 a 20% discount.
    public decimal Modifier { get; private set; }

    // Lowest number wins where bands overlap.
    public int Priority { get; private set; }

    public void Update(string name, TimeOnly start, TimeOnly end, decimal modifier, int priority)
    {
        Name = EnsureValidName(name);
        (Start, End) = EnsureValidWindow(start, end);
        Modifier = EnsureValidModifier(modifier);
        Priority = priority;
    }

    public bool Covers(TimeOnly time) => time >= Start && time < End;

    public decimal RateFor(decimal baseHourlyRate) => baseHourlyRate * (1 + Modifier);

    private static string EnsureValidName(string name) =>
        string.IsNullOrWhiteSpace(name)
            ? throw new ArgumentException("Band name is required.", nameof(name))
            : name.Trim();

    // No midnight wrap.
    private static (TimeOnly Start, TimeOnly End) EnsureValidWindow(TimeOnly start, TimeOnly end) =>
        end <= start
            ? throw new ArgumentException($"Band end ({end}) must be after start ({start}).", nameof(end))
            : (start, end);

    // At -1 the hall is free; below it the customer gets paid to book.
    private static decimal EnsureValidModifier(decimal modifier) =>
        modifier <= -1
            ? throw new ArgumentOutOfRangeException(nameof(modifier), modifier, "Modifier must be greater than -1.")
            : modifier;
}
