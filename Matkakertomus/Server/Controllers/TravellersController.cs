using Matkakertomus.Server.Services;
using Matkakertomus.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Matkakertomus.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TravellersController : ControllerBase
    {
        private readonly ITravellerService _service;

        public TravellersController(ITravellerService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProfileDto>>> GetTravellers()
        {
            return Ok(await _service.GetAllTravellersAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProfileDto>> GetTraveller(uint id)
        {
            var traveller = await _service.GetTravellerByIdAsync(id);
            if (traveller == null) return NotFound();
            return Ok(traveller);
        }

        [Authorize]
        [HttpGet("profile")]
        public async Task<ActionResult<ProfileDto>> GetProfile()
        {
            var userId = uint.Parse(User.FindFirst("id")?.Value);
            var user = await _service.GetTravellerByIdAsync(userId);
            if (user == null) return NotFound();

            return Ok(user);
        }

        [Authorize]
        [HttpPut("profile")]
        public async Task<IActionResult> PutTraveller(ProfileDto traveller)
        {
            var userId = uint.Parse(User.FindFirst("id")?.Value);
            var user = await _service.GetTravellerByIdAsync(userId);

            if (user == null) return NotFound();

            await _service.UpdateTravellerAsync(userId, traveller);

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<ProfileDto>> PostTraveller(ProfileDto traveller)
        {
            await _service.CreateTravellerAsync(traveller);

            return CreatedAtAction("GetTraveller", new { id = traveller.TravellerId }, traveller);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTraveller(uint id)
        {
            var result = await _service.DeleteTravellerAsync(id);

            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
