using ConferenceBooking.Application.Interfaces;
using ConferenceBooking.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ConferenceBooking.Infrastructure.Persistence.Repositories;

public class AmenityRepository : IAmenityRepository
{
    private readonly ApplicationDbContext _context;

    public AmenityRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    // Tracked: these get attached to a hall or a booking.
    public Task<Amenity?> GetByIdAsync(Guid id, CancellationToken cancellationToken) =>
        _context.Amenities.FirstOrDefaultAsync(a => a.Id == id, cancellationToken);

    public async Task<IReadOnlyList<Amenity>> GetByIdsAsync(
        IEnumerable<Guid> ids,
        CancellationToken cancellationToken) =>
        await _context.Amenities
            .Where(a => ids.Contains(a.Id))
            .ToListAsync(cancellationToken);
}
