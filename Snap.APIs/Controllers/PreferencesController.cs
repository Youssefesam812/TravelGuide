using Microsoft.AspNetCore.Mvc;
using Snap.APIs.DTOs;
using Snap.Core.Entities;
using Snap.Repository.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Snap.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PreferencesController : ControllerBase
    {
        private readonly SnapDbContext _context;

        public PreferencesController(SnapDbContext context)
        {
            _context = context;
        }

        // GET: api/Preferences/{userId}
        [HttpGet("{userId}")]
        public async Task<ActionResult<PreferencesDto>> GetPreferences(string userId)
        {
            var preferences = await _context.Preferences
                .FirstOrDefaultAsync(p => p.UserId == userId);

            if (preferences == null)
            {
                return NotFound("Preferences not found for this user.");
            }

            return new PreferencesDto
            {
                PreferredPlaces = preferences.PreferredPlaces,
                TravelTags = preferences.TravelTags,
                Age = preferences.Age,
                MaritalStatus = preferences.MaritalStatus,
                Children = preferences.Children,
                Gender = preferences.Gender
            };
        }

        // POST: api/Preferences/{userId}
        [HttpPost("{userId}")]
        public async Task<IActionResult> CreateOrUpdatePreferences(string userId, [FromBody] PreferencesDto dto)
        {
            var preferences = await _context.Preferences
                .FirstOrDefaultAsync(p => p.UserId == userId);

            if (preferences == null)
            {
                preferences = new Preferences
                {
                    UserId = userId,
                    PreferredPlaces = dto.PreferredPlaces,
                    TravelTags = dto.TravelTags,
                    Age = dto.Age,
                    MaritalStatus = dto.MaritalStatus,
                    Children = dto.Children,
                    Gender = dto.Gender
                };
                _context.Preferences.Add(preferences);
            }
            else
            {
                preferences.PreferredPlaces = dto.PreferredPlaces;
                preferences.TravelTags = dto.TravelTags;
                preferences.Age = dto.Age;
                preferences.MaritalStatus = dto.MaritalStatus;
                preferences.Children = dto.Children;
                preferences.Gender = dto.Gender;
                _context.Preferences.Update(preferences);
            }

            await _context.SaveChangesAsync();
            return Ok("Preferences saved successfully.");
        }

        // PUT: api/Preferences/{userId}
        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdatePreferences(string userId, [FromBody] PreferencesDto dto)
        {
            var preferences = await _context.Preferences
                .FirstOrDefaultAsync(p => p.UserId == userId);

            if (preferences == null)
            {
                return NotFound("Preferences not found for this user.");
            }

            // Update only the fields that are provided in the DTO
            if (dto.PreferredPlaces != null)
            {
                preferences.PreferredPlaces = dto.PreferredPlaces;
            }

            if (dto.TravelTags != null)
            {
                preferences.TravelTags = dto.TravelTags;
            }

            if (dto.Age.HasValue)
            {
                preferences.Age = dto.Age;
            }

            if (dto.MaritalStatus != null)
            {
                preferences.MaritalStatus = dto.MaritalStatus;
            }

            if (dto.Children != null)
            {
                preferences.Children = dto.Children;
            }

            if (dto.Gender != null)
            {
                preferences.Gender = dto.Gender;
            }

            _context.Preferences.Update(preferences);
            await _context.SaveChangesAsync();

            return Ok("Preferences updated successfully.");
        }
    }
}