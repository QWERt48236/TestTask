using ConferenceBooking.Domain.Entities;

namespace ConferenceBooking.Application.Dtos;

public static class HallMappings
{
    public static HallDto ToDto(this Hall hall) => new()
    {
        Id = hall.Id,
        Name = hall.Name,
        Capacity = hall.Capacity,
        BaseHourlyRate = hall.BaseHourlyRate,
        // By name, not id: GUIDs sort arbitrarily, so ordering by them would shuffle
        // the list between halls for no reason a caller could predict.
        Amenities = [.. hall.Amenities.Select(a => a.ToDto()).OrderBy(a => a.Name)],
    };

    public static AmenityDto ToDto(this Amenity amenity) => new()
    {
        Id = amenity.Id,
        Name = amenity.Name,
        Price = amenity.Price,
    };
}
