using ConferenceBooking.Application.Dtos;
using ConferenceBooking.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ConferenceBooking.Api.Controllers;

[ApiController]
[Route("api/halls")]
public class HallsController : ControllerBase
{
    private readonly IHallService _halls;

    public HallsController(IHallService halls)
    {
        _halls = halls;
    }

    [HttpGet]
    [ProducesResponseType<IReadOnlyList<HallDto>>(StatusCodes.Status200OK)]
    public async Task<IReadOnlyList<HallDto>> GetAll(CancellationToken cancellationToken) =>
        await _halls.GetAllAsync(cancellationToken);

    [HttpGet("{id:guid}")]
    [ProducesResponseType<HallDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<HallDto> GetById(Guid id, CancellationToken cancellationToken) =>
        await _halls.GetByIdAsync(id, cancellationToken);

    [HttpPost]
    [ProducesResponseType<HallDto>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<HallDto>> Create(
        CreateHallRequest request,
        CancellationToken cancellationToken)
    {
        var hall = await _halls.CreateAsync(request, cancellationToken);

        return CreatedAtAction(nameof(GetById), new { id = hall.Id }, hall);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType<HallDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<HallDto> Update(
        Guid id,
        UpdateHallRequest request,
        CancellationToken cancellationToken) =>
        await _halls.UpdateAsync(id, request, cancellationToken);

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await _halls.DeleteAsync(id, cancellationToken);

        return NoContent();
    }

    [HttpPost("{id:guid}/amenities/{amenityId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> AddAmenity(Guid id, Guid amenityId, CancellationToken cancellationToken)
    {
        await _halls.AddAmenityAsync(id, amenityId, cancellationToken);

        return NoContent();
    }

    [HttpDelete("{id:guid}/amenities/{amenityId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoveAmenity(Guid id, Guid amenityId, CancellationToken cancellationToken)
    {
        await _halls.RemoveAmenityAsync(id, amenityId, cancellationToken);

        return NoContent();
    }
}
