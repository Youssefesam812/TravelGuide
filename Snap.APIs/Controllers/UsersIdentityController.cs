using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Snap.APIs.DTOs;
using Snap.APIs.Errors;
using Snap.Core.Entities;
using Snap.Core.Services;

namespace Snap.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersIdentityController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ITokenService _tokenService;

        public UsersIdentityController(UserManager<User>userManager ,
            SignInManager<User> signInManager , 
            ITokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
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
    }
}
