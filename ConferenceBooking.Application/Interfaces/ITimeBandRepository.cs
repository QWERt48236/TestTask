using ConferenceBooking.Domain.Entities;

namespace ConferenceBooking.Application.Interfaces;

public interface ITimeBandRepository
{
    Task<IReadOnlyList<TimeBand>> GetAllAsync(CancellationToken cancellationToken);
}
