using ConferenceBooking.Application.Dtos;

namespace ConferenceBooking.Application.Interfaces;

public interface IHallService
{
    Task<IReadOnlyList<HallDto>> GetAllAsync(CancellationToken cancellationToken);

    Task<HallDto> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<HallDto> CreateAsync(CreateHallRequest request, CancellationToken cancellationToken);

    Task<HallDto> UpdateAsync(Guid id, UpdateHallRequest request, CancellationToken cancellationToken);

    Task DeleteAsync(Guid id, CancellationToken cancellationToken);

    Task AddAmenityAsync(Guid hallId, Guid amenityId, CancellationToken cancellationToken);

    Task RemoveAmenityAsync(Guid hallId, Guid amenityId, CancellationToken cancellationToken);
}
