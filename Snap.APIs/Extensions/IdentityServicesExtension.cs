using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Snap.Core.Entities;
using Snap.Core.Services;
using Snap.Repository.Data;
using Snap.Service.Token;
using System.Security.Principal;

namespace Snap.APIs.Extensions
{
    public static class IdentityServicesExtension
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection Services) 
        {



            Services.AddScoped<ITokenService, TokenService>();


        Services.AddIdentity<User , IdentityRole>()
                .AddEntityFrameworkStores<SnapDbContext>();


            Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();
        
        
        
        return Services;
        
        
        
        }








    }
}
