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

    }
}
