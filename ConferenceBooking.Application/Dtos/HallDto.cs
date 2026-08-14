namespace ConferenceBooking.Application.Dtos;

public class HallDto
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public int Capacity { get; set; }

    public decimal BaseHourlyRate { get; set; }

    public IReadOnlyList<AmenityDto> Amenities { get; set; } = [];
}
