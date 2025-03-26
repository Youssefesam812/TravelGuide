using Microsoft.AspNetCore.Mvc;
using Snap.Repository.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using System.Text.Json;


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
        public async Task<ActionResult<IEnumerable<Governorate>>> GetGovernorates()
        {
            var governorates = await _context.Governorates.Include(g => g.TopPlaces).ToListAsync();

            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                WriteIndented = true
            };

            var jsonString = JsonSerializer.Serialize(governorates, options);
            return Content(jsonString, "application/json");
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Governorate>> GetGovernorateById(int id)
        {
            var governorate = await _context.Governorates.Include(g => g.TopPlaces)
                                                           .FirstOrDefaultAsync(g => g.Id == id);
            if (governorate == null)
            {
                return NotFound();
            }
            return Ok(governorate);
        }
    }
}
