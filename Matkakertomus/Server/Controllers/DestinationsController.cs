using Matkakertomus.Server.Services;
using Matkakertomus.Shared;
using Microsoft.AspNetCore.Mvc;

namespace Matkakertomus.Server.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class DestinationsController : ControllerBase
	{
		private readonly IDestinationService _repository;


		public DestinationsController(IDestinationService repository)
		{
			_repository = repository;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<DestinationDto>>> GetDestinations()
		{
			return Ok(await _repository.GetAllDestinationsAsync());
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<DestinationDto>> GetDestination(uint id)
		{
			var destination = await _repository.GetDestinationByIdAsync(id);
			if (destination == null) return NotFound();
			return Ok(destination);
		}


		[HttpPut("{id}")]
		public async Task<IActionResult> PutDestination(uint id, DestinationDto data)
		{
			var success = await _repository.UpdateDestinationAsync(id, data);
			if (!success) return NotFound();
			return NoContent();
		}

		[HttpPost]
		public async Task<ActionResult<DestinationDto>> PostDestination(DestinationDto destination)
		{
			await _repository.CreateDestinationAsync(destination);

			return CreatedAtAction("GetDestination", new { id = destination.DestinationId }, destination);
		}

		// DELETE: api/Destinations/5
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteDestination(uint id)
		{
			var success = await _repository.DeleteDestinationAsync(id);

			if (!success) return NotFound();

			return NoContent();
		}

	}
}
