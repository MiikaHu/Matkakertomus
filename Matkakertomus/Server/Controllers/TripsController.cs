using Matkakertomus.Server.Services;
using Matkakertomus.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Matkakertomus.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TripsController : ControllerBase
{
    private readonly ITripService _service;

    public TripsController(ITripService service)
    {
        _service = service;
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<IEnumerable<TripDto>>> GetAllTrips()
    {
        var trips = await _service.GetAllTripsAsync();
        return Ok(trips.Where(t => t.Private == 0));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TripDto>> GetTrip(uint id)
    {
        var trip = await _service.GetTripByIdAsync(id);
        if (trip == null) return NotFound();
        return Ok(trip);
    }
    [HttpGet("search")]
    public async Task<ActionResult<TripDto>> GetTripById()
    {
        var userId = uint.Parse(User.FindFirst("id")?.Value);
        var trips = await _service.GetAllTripsByTravellerIdAsync(userId);
        return Ok(trips);
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> PutTrip(uint id, TripDto data)
    {
        var success = await _service.UpdateTripAsync(id, data);
        if (!success) return NotFound();
        return NoContent();
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<TripDto>> PostTrip(TripDto tripDto)
    {
        var userId = uint.Parse(User.FindFirst("id")?.Value);
        tripDto.TravellerId = userId;
        var trip = await _service.CreateTripAsync(tripDto);

        return CreatedAtAction(nameof(GetTrip), new { id = trip.TripId }, tripDto);
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTrip(uint id)
    {
        var success = await _service.DeleteTripAsync(id);
        if (!success) return NotFound();
        return NoContent();
    }
}