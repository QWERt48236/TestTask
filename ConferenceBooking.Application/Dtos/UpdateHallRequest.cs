using System.ComponentModel.DataAnnotations;

namespace ConferenceBooking.Application.Dtos;

// Amenities are not here: they have their own endpoints.
public class UpdateHallRequest
{
    [Required]
    [StringLength(100, MinimumLength = 1)]
    public string Name { get; set; } = string.Empty;

    [Range(1, 10_000)]
    public int Capacity { get; set; }

    [Range(0.01, 1_000_000)]
    public decimal BaseHourlyRate { get; set; }
}
