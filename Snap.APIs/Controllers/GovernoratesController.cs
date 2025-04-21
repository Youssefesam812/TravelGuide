using Microsoft.AspNetCore.Mvc;
using Snap.Repository.Data;
using Microsoft.EntityFrameworkCore;
using Snap.APIs.DTOs;


namespace Snap.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GovernoratesController : ControllerBase
    {
        private readonly SnapDbContext  _context;

        public GovernoratesController(SnapDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GovernorateDto>>> GetGovernorates()
        {
            var governorates = await _context.Governorates
                .Include(g => g.TopPlaces)
                .Select(g => new GovernorateDto
                {
                    Id = g.Id,
                    Name = g.Name,
                    ImageUrl = g.ImageUrl,
                    Description = g.Description,
                    TopPlaces = g.TopPlaces.Select(tp => new TopPlaceDto
                    {
                        Id = tp.Id,
                        Name = tp.Name,
                        ImageUrl = tp.ImageUrl
                    }).ToList()
                })
                .ToListAsync();

            return Ok(governorates);
        }



        [HttpGet("{id}")]
        public async Task<ActionResult<GovernorateDto>> GetGovernorateById(int id)
        {
            var governorate = await _context.Governorates
                .Include(g => g.TopPlaces)
                .Where(g => g.Id == id)
                .Select(g => new GovernorateDto
                {
                    Id = g.Id,
                    Name = g.Name,
                    ImageUrl = g.ImageUrl,
                    Description = g.Description,
                    TopPlaces = g.TopPlaces.Select(tp => new TopPlaceDto
                    {
                        Id = tp.Id,
                        Name = tp.Name,
                        ImageUrl = tp.ImageUrl
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            if (governorate == null)
            {
                return NotFound();
            }

            return Ok(governorate);
        }

        // POST: api/Governorates
        [HttpPost]
        public async Task<ActionResult<GovernorateDto>> CreateGovernorate([FromBody] GovernorateCreateDto governorateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var governorate = new Governorate
            {
                Name = governorateDto.Name,
                ImageUrl = governorateDto.ImageUrl,
                Description = governorateDto.Description,
                TopPlaces = governorateDto.TopPlaces?.Select(tp => new TopPlace
                {
                    Name = tp.Name,
                    ImageUrl = tp.ImageUrl
                }).ToList() ?? new List<TopPlace>()
            };

            _context.Governorates.Add(governorate);
            await _context.SaveChangesAsync();

            var result = new GovernorateDto
            {
                Id = governorate.Id,
                Name = governorate.Name,
                ImageUrl = governorate.ImageUrl,
                Description = governorate.Description,
                TopPlaces = governorate.TopPlaces.Select(tp => new TopPlaceDto
                {
                    Id = tp.Id,
                    Name = tp.Name,
                    ImageUrl = tp.ImageUrl
                }).ToList()
            };

            return CreatedAtAction(nameof(GetGovernorateById), new { id = governorate.Id }, result);
        }

        // PUT: api/Governorates/id
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGovernorate(int id, [FromBody] GovernorateUpdateDto governorateDto)
        {
            var governorate = await _context.Governorates
                .Include(g => g.TopPlaces)
                .FirstOrDefaultAsync(g => g.Id == id);

            if (governorate == null)
            {
                return NotFound();
            }

            // Update governorate properties
            if (governorateDto.Name != null) governorate.Name = governorateDto.Name;
            if (governorateDto.ImageUrl != null) governorate.ImageUrl = governorateDto.ImageUrl;
            if (governorateDto.Description != null) governorate.Description = governorateDto.Description;

            // Handle top places updates
            if (governorateDto.TopPlaces != null)
            {
                // Remove existing places not in the update
                var placesToRemove = governorate.TopPlaces
                    .Where(existing => !governorateDto.TopPlaces.Any(newPlace => newPlace.Id == existing.Id))
                    .ToList();

                foreach (var place in placesToRemove)
                {
                    _context.TopPlaces.Remove(place);
                }

                // Update existing or add new places
                foreach (var placeDto in governorateDto.TopPlaces)
                {
                    if (placeDto.Id > 0) // Existing place
                    {
                        var existingPlace = governorate.TopPlaces.FirstOrDefault(p => p.Id == placeDto.Id);
                        if (existingPlace != null)
                        {
                            existingPlace.Name = placeDto.Name;
                            existingPlace.ImageUrl = placeDto.ImageUrl;
                        }
                    }
                    else // New place
                    {
                        governorate.TopPlaces.Add(new TopPlace
                        {
                            Name = placeDto.Name,
                            ImageUrl = placeDto.ImageUrl
                        });
                    }
                }
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GovernorateExists(id))
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }

        // DELETE: api/Governorates/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGovernorate(int id)
        {
            var governorate = await _context.Governorates.FindAsync(id);
            if (governorate == null)
            {
                return NotFound();
            }

            _context.Governorates.Remove(governorate);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool GovernorateExists(int id)
        {
            return _context.Governorates.Any(e => e.Id == id);
        }

        [HttpGet("ByName/{name}")]
        public async Task<ActionResult<string>> GetGovernorateImageUrlByName(string name)
        {
            var imageUrl = await _context.Governorates
                .Where(g => g.Name.ToLower() == name.ToLower())
                .Select(g => g.ImageUrl)
                .FirstOrDefaultAsync();

            if (imageUrl == null)
            {
                return NotFound();
            }

            return Ok(imageUrl);
        }
    }



}

