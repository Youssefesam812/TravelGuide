using Microsoft.AspNetCore.Mvc;
using Snap.Repository.Data;
using Microsoft.EntityFrameworkCore;


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
            return Ok(governorates);
        }
    }
}
