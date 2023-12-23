using Core.Identity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.Token_Service
{
    public interface ITokenService
    {
        string CreateToken(ApplicationUser appUser);
    }
}
