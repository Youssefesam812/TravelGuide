using Microsoft.AspNetCore.Mvc;
using Snap.APIs.DTOs;
using Snap.Core.Entities;
using Snap.Repository.Data;
using Microsoft.EntityFrameworkCore;

namespace Snap.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TripDaysController : ControllerBase
    {
        private readonly SnapDbContext _context;

        public TripDaysController(SnapDbContext context)
        {
            _context = context;
        }

        // GET: api/TripDays/ByTripPlan/5
        [HttpGet("ByTripPlan/{tripPlanId}")]
        public async Task<ActionResult<IEnumerable<TripDayDto>>> GetTripDaysByTripPlan(int tripPlanId)
        {
            var tripDays = await _context.TripDays
                .Include(td => td.Activities)
                .Where(td => td.TripPlanId == tripPlanId)
                .ToListAsync();

            if (!tripDays.Any())
            {
                return NotFound();
            }

            var tripDayDtos = tripDays.Select(td => new TripDayDto
            {
                Day = td.Day,
                ApproximateCost = td.ApproximateCost,
                Activities = td.Activities.Select(a => new TripActivityDto
                {
                    Activity = a.Activity,
                    Location = a.Location,
                    PriceRange = a.PriceRange,
                    Time = a.Time
                }).ToList(),
                Notes = td.Notes
            }).ToList();

            return Ok(tripDayDtos);
        }

        // POST: api/TripDays
        [HttpPost]
        public async Task<ActionResult<TripDay>> PostTripDay(TripDayDto tripDayDto)
        {
            var tripDay = new TripDay
            {
                Day = tripDayDto.Day,
                ApproximateCost = tripDayDto.ApproximateCost,
                TripPlanId = tripDayDto.TripPlanId,
                Activities = tripDayDto.Activities.Select(a => new TripActivity
                {
                    Activity = a.Activity,
                    Location = a.Location,
                    PriceRange = a.PriceRange,
                    Time = a.Time
                }).ToList(),
                Notes = tripDayDto.Notes
            };

            _context.TripDays.Add(tripDay);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTripDay", new { id = tripDay.Id }, tripDay);
        }

        // GET: api/TripDays/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TripDay>> GetTripDay(int id)
        {
            var tripDay = await _context.TripDays
                .Include(td => td.Activities)
                .FirstOrDefaultAsync(td => td.Id == id);

            if (tripDay == null)
            {
                return NotFound();
            }

            return tripDay;
        }
    }
}
