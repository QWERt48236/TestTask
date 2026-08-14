using ConferenceBooking.Application.Pricing;
using ConferenceBooking.Domain.Entities;

namespace ConferenceBooking.Application.Interfaces;

public interface IPricingService
{
    Task<PriceBreakdown> CalculateAsync(
        Hall hall,
        DateTime start,
        TimeSpan duration,
        IReadOnlyCollection<Amenity> amenities,
        CancellationToken cancellationToken);
}
