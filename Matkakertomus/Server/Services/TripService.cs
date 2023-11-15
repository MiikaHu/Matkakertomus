using Matkakertomus.Server.Models;
using Matkakertomus.Shared;
using Microsoft.EntityFrameworkCore;

namespace Matkakertomus.Server.Services;

public class TripService : ITripService
{
	private readonly TravelContext _dbContext;

	public TripService(TravelContext dbContext)
	{
		_dbContext = dbContext;
	}

	public async Task<IEnumerable<TripDto>> GetAllTripsAsync()
	{
		return await _dbContext.Trips.Select(d => d.ToDto()).ToListAsync();

	}

	public async Task<TripDto?> GetTripByIdAsync(uint id)
	{
		var trip = await _dbContext.Trips.FindAsync(id);
		return trip?.ToDto();
	}
	public async Task<IEnumerable<TripDto>> GetAllTripsByTravellerIdAsync(uint id)
	{
        var trips = await _dbContext.Trips.Where(t => t.TravellerId == id)
                     .Select(t => new TripDto
                     {
                         TripId = t.TripId,
                         StartDate = t.StartDate,
                         EndDate = t.EndDate,
                         Private = t.Private,
                         TravellerId = t.TravellerId,
                         Title = t.Title,
                     })
                     .ToListAsync();
		return trips;
    }
    public async Task<TripDto> CreateTripAsync(TripDto trip)
	{
		_dbContext.Trips.Add(trip.ToTrip());
		await _dbContext.SaveChangesAsync();
		return trip;
	}

	public async Task<bool> UpdateTripAsync(uint id, TripDto updatedValues)
	{
		var tripToUpdate = await _dbContext.Trips.FindAsync(id);
		if (tripToUpdate == null) return false;

		tripToUpdate.EndDate = updatedValues.EndDate;
		tripToUpdate.StartDate = updatedValues.StartDate;
		tripToUpdate.Title = updatedValues.Title;
		tripToUpdate.Private = updatedValues.Private;

		await _dbContext.SaveChangesAsync();
		return true;
	}

	public async Task<bool> DeleteTripAsync(uint id)
	{
		var trip = await _dbContext.Trips.FindAsync(id);
		if (trip == null) return false;
		_dbContext.Trips.Remove(trip);
		await _dbContext.SaveChangesAsync();
		return true;
	}
}

public interface ITripService
{
	Task<IEnumerable<TripDto>> GetAllTripsAsync();
	Task<TripDto?> GetTripByIdAsync(uint id);
	Task<TripDto> CreateTripAsync(TripDto trip);
	Task<IEnumerable<TripDto>> GetAllTripsByTravellerIdAsync(uint id);
	Task<bool> UpdateTripAsync(uint id, TripDto updatedValues);
	Task<bool> DeleteTripAsync(uint id);
}
