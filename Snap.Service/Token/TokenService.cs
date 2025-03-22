using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Snap.Core.Entities;
using Snap.Core.Services;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Snap.Service.Token
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration configuration;

        public TokenService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }




        public async Task<string> CreateTokenAsync(User user , UserManager<User>userManager)
        {

            var AuthClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.GivenName , user.DispalyName),

                new Claim(ClaimTypes.Email , user.Email)
            };
            var UserRoles = await userManager.GetRolesAsync(user);

            foreach (var Role in UserRoles)
            {
                AuthClaims.Add(new Claim(ClaimTypes.Role , Role));
            }



            var AuthKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]));
            var Token = new JwtSecurityToken(
              issuer: configuration["JWT:ValidIssuer"],
              audience: configuration["JWT:ValidAudience"],
              expires: DateTime.Now.AddDays(double.Parse(configuration["JWT:DurationInDays"])),
               claims : AuthClaims ,
               signingCredentials : new SigningCredentials(AuthKey , SecurityAlgorithms.HmacSha256Signature));
            return new JwtSecurityTokenHandler().WriteToken(Token);
        }
    }
}
