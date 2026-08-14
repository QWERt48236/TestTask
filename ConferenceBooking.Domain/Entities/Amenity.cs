namespace ConferenceBooking.Domain.Entities;

public class Amenity
{
    private Amenity()
    {
        Name = string.Empty;
    }

    public Amenity(string name, decimal price)
    {
        Id = Guid.NewGuid();
        Name = EnsureValidName(name);
        Price = EnsureValidPrice(price);
    }

    public Guid Id { get; private set; }

    public string Name { get; private set; }

    // Flat fee per booking, not per hour.
    public decimal Price { get; private set; }

    public void Update(string name, decimal price)
    {
        Name = EnsureValidName(name);
        Price = EnsureValidPrice(price);
    }

    private static string EnsureValidName(string name) =>
        string.IsNullOrWhiteSpace(name)
            ? throw new ArgumentException("Amenity name is required.", nameof(name))
            : name.Trim();

    private static decimal EnsureValidPrice(decimal price) =>
        price < 0
            ? throw new ArgumentOutOfRangeException(nameof(price), price, "Amenity price cannot be negative.")
            : price;
}
