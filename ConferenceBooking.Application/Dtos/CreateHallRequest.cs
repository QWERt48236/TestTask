using System.ComponentModel.DataAnnotations;

namespace ConferenceBooking.Application.Dtos;

public class CreateHallRequest
{
    [Required]
    [StringLength(100, MinimumLength = 1)]
    public string Name { get; set; } = string.Empty;

    [Range(1, 10_000)]
    public int Capacity { get; set; }

    [Range(0.01, 1_000_000)]
    public decimal BaseHourlyRate { get; set; }

    public IReadOnlyList<Guid> AmenityIds { get; set; } = [];
}
