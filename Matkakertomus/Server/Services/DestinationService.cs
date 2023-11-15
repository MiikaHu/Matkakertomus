using Matkakertomus.Server.Models;
using Matkakertomus.Shared;
using Microsoft.EntityFrameworkCore;

namespace Matkakertomus.Server.Services;

public class DestinationService : IDestinationService
{
	private readonly TravelContext _dbContext;

	public DestinationService(TravelContext dbContext)
	{
		_dbContext = dbContext;
	}

	public async Task<IEnumerable<DestinationDto>> GetAllDestinationsAsync()
	{
		return await _dbContext.Destinations.Select(d => d.ToDto()).ToListAsync();
	}

	public async Task<DestinationDto?> GetDestinationByIdAsync(uint id)
	{
		var destination = await _dbContext.Destinations.FindAsync(id);
		return destination?.ToDto();
	}


	public async Task<DestinationDto> CreateDestinationAsync(DestinationDto destination)
	{
		_dbContext.Destinations.Add(destination.ToDestination());
		await _dbContext.SaveChangesAsync();
		return destination;
	}

	public async Task<bool> UpdateDestinationAsync(uint id, DestinationDto updatedValues)
	{
		var destinationToUpdate = await _dbContext.Destinations.FindAsync(id);
		if (destinationToUpdate == null) return false;

		destinationToUpdate.DestinationName = updatedValues.DestinationName;
		destinationToUpdate.Country = updatedValues.Country;
		destinationToUpdate.Area = updatedValues.Area;
		destinationToUpdate.Description = updatedValues.Description;
		destinationToUpdate.Image = updatedValues.Image;

		await _dbContext.SaveChangesAsync();
		return true;
	}


	public async Task<bool> DeleteDestinationAsync(uint id)
	{
		var destination = await _dbContext.Destinations.FindAsync(id);
		if (destination == null) return false;
		_dbContext.Destinations.Remove(destination);
		await _dbContext.SaveChangesAsync();
		return true;
	}
}


public interface IDestinationService
{
	Task<IEnumerable<DestinationDto>> GetAllDestinationsAsync();
	Task<DestinationDto?> GetDestinationByIdAsync(uint id);
	Task<DestinationDto> CreateDestinationAsync(DestinationDto destination);
	Task<bool> UpdateDestinationAsync(uint id, DestinationDto updatedValues);
	Task<bool> DeleteDestinationAsync(uint id);
}
