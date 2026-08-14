using ConferenceBooking.Application.Dtos;
using ConferenceBooking.Application.Exceptions;
using ConferenceBooking.Application.Interfaces;
using ConferenceBooking.Domain.Entities;

namespace ConferenceBooking.Application.Services;

public class HallService : IHallService
{
    private readonly IHallRepository _halls;
    private readonly IAmenityRepository _amenities;

    public HallService(IHallRepository halls, IAmenityRepository amenities)
    {
        _halls = halls;
        _amenities = amenities;
    }

    public async Task<IReadOnlyList<HallDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        var halls = await _halls.GetAllAsync(cancellationToken);

        return [.. halls.Select(h => h.ToDto())];
    }

    public async Task<HallDto> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var hall = await GetRequiredHallAsync(id, cancellationToken);

        return hall.ToDto();
    }

    public async Task<HallDto> CreateAsync(CreateHallRequest request, CancellationToken cancellationToken)
    {
        await EnsureNameIsFreeAsync(request.Name, excludeId: null, cancellationToken);

        var hall = new Hall(request.Name, request.Capacity, request.BaseHourlyRate);

        foreach (var amenity in await GetRequiredAmenitiesAsync(request.AmenityIds, cancellationToken))
        {
            hall.Amenities.Add(amenity);
        }

        await _halls.AddAsync(hall, cancellationToken);
        await _halls.SaveChangesAsync(cancellationToken);

        return hall.ToDto();
    }

    public async Task<HallDto> UpdateAsync(Guid id, UpdateHallRequest request, CancellationToken cancellationToken)
    {
        var hall = await GetRequiredHallAsync(id, cancellationToken);

        await EnsureNameIsFreeAsync(request.Name, excludeId: id, cancellationToken);

        hall.Update(request.Name, request.Capacity, request.BaseHourlyRate);
        await _halls.SaveChangesAsync(cancellationToken);

        return hall.ToDto();
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var hall = await GetRequiredHallAsync(id, cancellationToken);

        // Checked here for a readable 409; the Restrict FK alone would give an opaque 500.
        if (await _halls.HasBookingsAsync(id, cancellationToken))
        {
            throw new ConflictException($"Hall '{hall.Name}' has bookings and cannot be deleted.");
        }

        _halls.Remove(hall);
        await _halls.SaveChangesAsync(cancellationToken);
    }

    public async Task AddAmenityAsync(Guid hallId, Guid amenityId, CancellationToken cancellationToken)
    {
        var hall = await GetRequiredHallAsync(hallId, cancellationToken);
        var amenity = await _amenities.GetByIdAsync(amenityId, cancellationToken)
            ?? throw NotFoundException.For("Amenity", amenityId);

        if (hall.Amenities.Any(a => a.Id == amenityId))
        {
            throw new ConflictException($"Hall '{hall.Name}' already offers '{amenity.Name}'.");
        }

        hall.Amenities.Add(amenity);
        await _halls.SaveChangesAsync(cancellationToken);
    }

    public async Task RemoveAmenityAsync(Guid hallId, Guid amenityId, CancellationToken cancellationToken)
    {
        var hall = await GetRequiredHallAsync(hallId, cancellationToken);
        var amenity = hall.Amenities.FirstOrDefault(a => a.Id == amenityId)
            ?? throw new NotFoundException($"Hall '{hall.Name}' does not offer amenity with id {amenityId}.");

        hall.Amenities.Remove(amenity);
        await _halls.SaveChangesAsync(cancellationToken);
    }

    private async Task<Hall> GetRequiredHallAsync(Guid id, CancellationToken cancellationToken) =>
        await _halls.GetByIdAsync(id, cancellationToken)
            ?? throw NotFoundException.For("Hall", id);

    // Up front so a duplicate name is a 409, not a unique-index violation.
    private async Task EnsureNameIsFreeAsync(string name, Guid? excludeId, CancellationToken cancellationToken)
    {
        if (await _halls.NameExistsAsync(name, excludeId, cancellationToken))
        {
            throw new ConflictException($"A hall named '{name}' already exists.");
        }
    }

    private async Task<IReadOnlyList<Amenity>> GetRequiredAmenitiesAsync(
        IReadOnlyList<Guid> amenityIds,
        CancellationToken cancellationToken)
    {
        if (amenityIds.Count == 0)
        {
            return [];
        }

        var requested = amenityIds.Distinct().ToList();
        var found = await _amenities.GetByIdsAsync(requested, cancellationToken);

        var missing = requested.Except(found.Select(a => a.Id)).ToList();
        if (missing.Count > 0)
        {
            throw new NotFoundException($"Amenities not found: {string.Join(", ", missing)}.");
        }

        return found;
    }
}
