using Microsoft.AspNetCore.Identity;
using Snap.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snap.Core.Services
{
    public interface ITokenService
    {
        Task<string> CreateTokenAsync(User user , UserManager<User> userManager);

    }
}
