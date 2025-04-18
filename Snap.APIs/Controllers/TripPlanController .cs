using Microsoft.AspNetCore.Mvc;
using Snap.APIs.DTOs;
using Snap.Core.Entities;
using Snap.Repository.Data;
using Microsoft.EntityFrameworkCore;


namespace Snap.APIs.Controllers
{
        [ApiController]
        [Route("api/[controller]")]
        public class TripPlanController : ControllerBase
        {
            private readonly SnapDbContext _context;

            public TripPlanController(SnapDbContext context)
            {
                _context = context;
            }


        // POST: api/TripPlan
        [HttpPost]
        public async Task<IActionResult> CreateTripPlan([FromBody] TripPlanDto tripPlanDto)
        {
           
                
            var tripPlan = new TripPlan
            {
                City = tripPlanDto.City,
                Image = tripPlanDto.Image,
                VisitorType = tripPlanDto.VisitorType,
                NumDays = tripPlanDto.NumDays,
                Budget = tripPlanDto.Budget,
                UserId = tripPlanDto.UserId,
            };

            _context.TripPlans.Add(tripPlan);
            await _context.SaveChangesAsync();

            var resultDto = new TripPlanDto
            {
                City = tripPlan.City,
                Image = tripPlan.Image,
                VisitorType = tripPlan.VisitorType,
                NumDays = tripPlan.NumDays,
                Budget = tripPlan.Budget,
            };

            return Ok(resultDto);
        }

        // GET: api/TripPlan - Get all trip plans
        [HttpGet]
        public async Task<ActionResult<List<TripPlanDto>>> GetAllTripPlans()
        {
            var tripPlans = await _context.TripPlans
                .Select(t => new TripPlanDto
                {
                    City = t.City,
                    Image = t.Image,
                    VisitorType = t.VisitorType,
                    NumDays = t.NumDays,
                    Budget = t.Budget,
                    UserId = t.UserId
                })
                .ToListAsync();

            return Ok(tripPlans);
        }

        // GET: api/TripPlan/user/{userId}
        [HttpGet("user/{userId}")]
            public async Task<ActionResult<List<TripPlan>>> GetTripPlansByUserId(string userId)
            {
            var tripPlans = await _context.TripPlans
                .Where(t => t.UserId == userId)
                .Select(t => new TripPlanDto
                {
                    City = t.City,
                    Image = t.Image,
                    VisitorType = t.VisitorType,
                    NumDays = t.NumDays,
                    Budget = t.Budget,
                    
                })
                .ToListAsync();

            return Ok(tripPlans);
        }

            // GET: api/TripPlan/{id}/user/{userId} - Get specific trip with both ID and user ID
            [HttpGet("{id}/user/{userId}")]
            public async Task<ActionResult<TripPlan>> GetTripPlanByIdAndUserId(int id, string userId)
            {
                var tripPlan = await _context.TripPlans
                    .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

                if (tripPlan == null) return NotFound();
                return tripPlan;
            }
        }
}

