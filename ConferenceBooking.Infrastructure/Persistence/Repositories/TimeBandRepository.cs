using ConferenceBooking.Application.Interfaces;
using ConferenceBooking.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ConferenceBooking.Infrastructure.Persistence.Repositories;

public class TimeBandRepository : ITimeBandRepository
{
    private readonly ApplicationDbContext _context;

    public TimeBandRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<TimeBand>> GetAllAsync(CancellationToken cancellationToken) =>
        await _context.TimeBands
            .AsNoTracking()
            .OrderBy(b => b.Priority)
            .ToListAsync(cancellationToken);
}
