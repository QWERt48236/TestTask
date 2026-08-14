using ConferenceBooking.Application.Interfaces;
using ConferenceBooking.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ConferenceBooking.Infrastructure.Persistence.Repositories;

public class HallRepository : IHallRepository
{
    private readonly ApplicationDbContext _context;

    public HallRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    // Read-only: the results only ever feed DTOs.
    public async Task<IReadOnlyList<Hall>> GetAllAsync(CancellationToken cancellationToken) =>
        await _context.Halls
            .AsNoTracking()
            .Include(h => h.Amenities)
            .OrderBy(h => h.Name)
            .ToListAsync(cancellationToken);

    // Tracked, because callers mutate what they get back.
    public Task<Hall?> GetByIdAsync(Guid id, CancellationToken cancellationToken) =>
        _context.Halls
            .Include(h => h.Amenities)
            .FirstOrDefaultAsync(h => h.Id == id, cancellationToken);

    public Task<bool> NameExistsAsync(string name, Guid? excludeId, CancellationToken cancellationToken) =>
        _context.Halls
            .AsNoTracking()
            .AnyAsync(h => h.Name == name && (excludeId == null || h.Id != excludeId), cancellationToken);

    public Task<bool> HasBookingsAsync(Guid hallId, CancellationToken cancellationToken) =>
        _context.Bookings
            .AsNoTracking()
            .AnyAsync(b => b.HallId == hallId, cancellationToken);

    public async Task AddAsync(Hall hall, CancellationToken cancellationToken) =>
        await _context.Halls.AddAsync(hall, cancellationToken);

    public void Remove(Hall hall) => _context.Halls.Remove(hall);

    public Task SaveChangesAsync(CancellationToken cancellationToken) =>
        _context.SaveChangesAsync(cancellationToken);
}
