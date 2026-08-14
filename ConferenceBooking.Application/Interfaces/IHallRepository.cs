using ConferenceBooking.Domain.Entities;

namespace ConferenceBooking.Application.Interfaces;

public interface IHallRepository
{
    Task<IReadOnlyList<Hall>> GetAllAsync(CancellationToken cancellationToken);

    Task<Hall?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    // excludeId lets an update ignore the hall being renamed.
    Task<bool> NameExistsAsync(string name, Guid? excludeId, CancellationToken cancellationToken);

    Task<bool> HasBookingsAsync(Guid hallId, CancellationToken cancellationToken);

    Task AddAsync(Hall hall, CancellationToken cancellationToken);

    void Remove(Hall hall);

    Task SaveChangesAsync(CancellationToken cancellationToken);
}
