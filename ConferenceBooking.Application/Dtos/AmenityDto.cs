namespace ConferenceBooking.Application.Dtos;

public class AmenityDto
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public decimal Price { get; set; }
}
