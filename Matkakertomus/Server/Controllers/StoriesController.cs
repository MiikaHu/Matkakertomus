using Matkakertomus.Server.Models;
using Matkakertomus.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Matkakertomus.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoriesController : ControllerBase
    {
        private readonly TravelContext _context;

        public StoriesController(TravelContext context)
        {
            _context = context;
        }

        // GET: api/Stories
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<StoryDto>>> GetStories()
        {
            var stories = await _context.Stories.ToListAsync();
            return Ok(stories);
        }

        // GET: api/Stories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<StoryDto>> GetStory(uint id)
        {
            var ID = id;
            var stories = await _context.Stories.Where(t => t.TripId == ID)
                     .Select(t => new StoryDto
                     {
                         StoryId = t.StoryId,
                         Date = t.Date,
                         Text = t.Text,
                         TripId = t.TripId,
                         DestinationId = t.DestinationId,
                     })
                     .ToListAsync();
            return Ok(stories);
        }

        // PUT: api/Stories/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStory(uint id, StoryDto data)
        {
            var story = _context.Stories.Where(e => e.StoryId == id).FirstOrDefault();
            if (story == null) return BadRequest();
            story.Date = data.Date;
            story.Text = data.Text;
            story.TripId = data.TripId;
            story.DestinationId = data.DestinationId;

            await _context.SaveChangesAsync();


            return NoContent();
        }

        // POST: api/Stories
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<StoryDto>> PostStory(StoryDto storyDto)
        {
            var destination = await _context.Destinations.FindAsync(storyDto.DestinationId);
            var trip = await _context.Trips.FindAsync(storyDto.TripId);

            var story = new Story
            {
                Date = storyDto.Date,
                Text = storyDto.Text,
                TripId = storyDto.TripId,
                DestinationId = storyDto.DestinationId,
            };

            _context.Stories.Add(story);
            await _context.SaveChangesAsync();
            return Ok();
        }

        // DELETE: api/Stories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStory(uint id)
        {
            if (_context.Stories == null)
            {
                return NotFound();
            }
            var story = await _context.Stories.FindAsync(id);
            if (story == null)
            {
                return NotFound();
            }

            _context.Stories.Remove(story);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool StoryExists(uint id)
        {
            return (_context.Stories?.Any(e => e.StoryId == id)).GetValueOrDefault();
        }
    }
}