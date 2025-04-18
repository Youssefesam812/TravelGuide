using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Snap.APIs.DTOs;
using Snap.APIs.Errors;
using Snap.Core.Entities;
using Snap.Core.Services;
using Snap.Repository.Data;

namespace Snap.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersIdentityController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly SnapDbContext _context;

        public UsersIdentityController(UserManager<User>userManager ,
            SignInManager<User> signInManager , 
            ITokenService tokenService,
            SnapDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _context = context;
        }

        //Register
        [HttpPost("Register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto model)
        {
            var existingUserByEmail = await _userManager.FindByEmailAsync(model.Email);
            if (existingUserByEmail != null)
            {
                return BadRequest(new ApiResponse(400, "An account is already registered with this email."));
            }

            var existingUserByDisplayName = _userManager.Users
        .FirstOrDefault(u => u.DispalyName.ToLower() == model.DispalyName.ToLower());
            if (existingUserByDisplayName != null)
            {
                return BadRequest(new ApiResponse(400, "Display name is already taken. Please choose another one."));
            }


            var user = new User()
            {
                DispalyName = model.DispalyName,
                Email = model.Email,
                UserName = model.Email.Split('@')[0],
              
            };

            var result = await _userManager.CreateAsync(user, model.password);
            if (!result.Succeeded) { return BadRequest(new ApiResponse(400)); }

            var ReturnedUser = new UserDto()
            {
                DispalyName = user.DispalyName,
                Email = user.Email,
            
                Token = await _tokenService.CreateTokenAsync(user , _userManager)
            };

            return Ok(ReturnedUser);
        }
        //login 
        [HttpPost ("Login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto model)
        {

            var User = await _userManager.FindByEmailAsync(model.Email);

            if (User is null) { return Unauthorized(new ApiResponse(401)); }  
        
          var result =   await _signInManager.CheckPasswordSignInAsync(User , model.Password , false);

            if (!result.Succeeded) { return Unauthorized(new ApiResponse(401)); }
            return Ok(
                  new UserDto() {
                      UserId = User.Id,
                      DispalyName = User.DispalyName,
                      Email = User.Email,
                      Token = await _tokenService.CreateTokenAsync(User, _userManager)
                  }
                
                
            );

        }

        // GET: api/Users/AllWithPreferences
        [HttpGet("AllWithPreferences")]
        public async Task<ActionResult<IEnumerable<UserWithPreferencesDto>>> GetAllUsersWithPreferences()
        {
            var users = await _userManager.Users.ToListAsync();
            var userDtos = new List<UserWithPreferencesDto>();

            foreach (var user in users)
            {
                var preferences = await _context.Preferences
                    .FirstOrDefaultAsync(p => p.UserId == user.Id);

                userDtos.Add(new UserWithPreferencesDto
                {
                    UserId = user.Id,
                    DisplayName = user.DispalyName,
                    Email = user.Email,
                    Preferences = preferences != null ? new PreferencesDto
                    {
                        PreferredPlaces = preferences.PreferredPlaces,
                        TravelTags = preferences.TravelTags,
                        Age = preferences.Age,
                        MaritalStatus = preferences.MaritalStatus,
                        Children = preferences.Children,
                        Gender = preferences.Gender
                    } : null
                });
            }

            return Ok(userDtos);
        }
    }

}
  