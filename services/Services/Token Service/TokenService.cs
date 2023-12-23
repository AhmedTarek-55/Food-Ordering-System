using Core.Identity.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Services.Services.Token_Service
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;
        private readonly SymmetricSecurityKey _key;

        public TokenService(IConfiguration config)
        {
            _config = config;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Token:Key"]));
        }

        public string CreateToken(ApplicationUser appUser)
        {
            // 1. defining claims
            var claims = new List<Claim>
            {
                new Claim (ClaimTypes.GivenName, appUser.DisplayName),
                new Claim (ClaimTypes.Email, appUser.Email)
            };

            // 2. defining hashing algorithm
            var credis = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256);

            // 3. defining token descriptor
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Issuer = _config["Token:Issuer"],
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = credis
            };

            // 4. defining token handler
            var tokenHandler = new JwtSecurityTokenHandler();

            // 5. creating token 
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
