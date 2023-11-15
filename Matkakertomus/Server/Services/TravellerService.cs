using Matkakertomus.Server.Models;
using Matkakertomus.Shared;
using Microsoft.EntityFrameworkCore;

namespace Matkakertomus.Server.Services;

public class TravellerService : ITravellerService
{
	private readonly TravelContext _dbContext;

	public TravellerService(TravelContext dbContext)
	{
		_dbContext = dbContext;
	}

	public async Task<IEnumerable<ProfileDto>> GetAllTravellersAsync()
	{
		return await _dbContext.Travellers.Select(d => d.ToDto()).ToListAsync();
	}

	public async Task<ProfileDto?> GetTravellerByIdAsync(uint id)
	{
		var traveller = await _dbContext.Travellers.FindAsync(id);
		return traveller?.ToDto();
	}

	public async Task<ProfileDto> CreateTravellerAsync(ProfileDto traveller)
	{
		_dbContext.Travellers.Add(traveller.ToTraveller());
		await _dbContext.SaveChangesAsync();
		return traveller;
	}

	public async Task<bool> UpdateTravellerAsync(uint id, ProfileDto updatedValues)
	{
		var travellerToUpdate = await _dbContext.Travellers.FindAsync(id);
		if (travellerToUpdate == null) return false;

		travellerToUpdate.Image = updatedValues.Image;
		travellerToUpdate.FirstName = updatedValues.FirstName;
		travellerToUpdate.LastName = updatedValues.LastName;
		travellerToUpdate.Username = updatedValues.Username;
		travellerToUpdate.Description = updatedValues.Description;
		travellerToUpdate.Area = updatedValues.Area;
		travellerToUpdate.Email = updatedValues.Email;

		await _dbContext.SaveChangesAsync();
		return true;
	}

	public async Task<bool> DeleteTravellerAsync(uint id)
	{
		var traveller = await _dbContext.Travellers.FindAsync(id);
		if (traveller == null) return false;
		_dbContext.Travellers.Remove(traveller);
		await _dbContext.SaveChangesAsync();
		return true;
	}
}



public interface ITravellerService
{
	Task<IEnumerable<ProfileDto>> GetAllTravellersAsync();
	Task<ProfileDto?> GetTravellerByIdAsync(uint id);
	Task<ProfileDto> CreateTravellerAsync(ProfileDto traveller);
	Task<bool> UpdateTravellerAsync(uint id, ProfileDto updatedValues);
	Task<bool> DeleteTravellerAsync(uint id);
}
