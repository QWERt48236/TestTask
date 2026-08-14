using ConferenceBooking.Domain.Entities;

namespace ConferenceBooking.Application.Interfaces;

public interface IAmenityRepository
{
    Task<Amenity?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<IReadOnlyList<Amenity>> GetByIdsAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken);
}
