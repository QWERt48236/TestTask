namespace ConferenceBooking.Domain.Entities;

public class Amenity
{
    private Amenity()
    {
        Name = string.Empty;
    }

    public Amenity(string name, decimal price)
    {
        Name = EnsureValidName(name);
        Price = EnsureValidPrice(price);
    }

    public int Id { get; private set; }

    public string Name { get; private set; }

    /// <summary>Flat fee charged once per booking, not per hour.</summary>
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

    // Free amenities are allowed; negative ones are not.
    private static decimal EnsureValidPrice(decimal price) =>
        price < 0
            ? throw new ArgumentOutOfRangeException(nameof(price), price, "Amenity price cannot be negative.")
            : price;
}
