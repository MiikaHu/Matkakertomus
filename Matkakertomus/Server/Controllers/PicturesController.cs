using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Matkakertomus.Server.Models;
using Matkakertomus.Shared;
using Microsoft.AspNetCore.Authorization;

namespace Matkakertomus.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PicturesController : ControllerBase
    {
        private readonly TravelContext _context;

        public PicturesController(TravelContext context)
        {
            _context = context;
        }

        // GET: api/Pictures
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Picture>>> GetPictures()
        {
          if (_context.Pictures == null)
          {
              return NotFound();
          }
            return await _context.Pictures.ToListAsync();
        }

        // GET: api/Pictures/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<ImageDto>> GetPicture(uint id)
        {
            var ID = id;
            var pictures = await _context.Pictures.Where(t => t.StoryId == ID)
                     .Select(t => new ImageDto
                     {
                         StoryId = t.StoryId,
                         Image = t.Image,
                         ImageId = t.ImageId
                     })
                     .ToListAsync();
            return Ok(pictures);
        }

        // PUT: api/Pictures/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPicture(uint id, Picture picture)
        {
            if (id != picture.ImageId)
            {
                return BadRequest();
            }

            _context.Entry(picture).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PictureExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Pictures
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Picture>> PostPicture(ImageDto image)
        {
            var picture = new Picture
            {
                Image = image.Image,
                StoryId= image.StoryId,
            };

            _context.Pictures.Add(picture);
            await _context.SaveChangesAsync();
            return Ok();
        }

        // DELETE: api/Pictures/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePicture(uint id)
        {
            if (_context.Pictures == null)
            {
                return NotFound();
            }
            var picture = await _context.Pictures.FindAsync(id);
            if (picture == null)
            {
                return NotFound();
            }

            _context.Pictures.Remove(picture);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PictureExists(uint id)
        {
            return (_context.Pictures?.Any(e => e.ImageId == id)).GetValueOrDefault();
        }
    }
}
