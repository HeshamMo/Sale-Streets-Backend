using SaleStreets_Back_end.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SaleStreets_Back_end.Services.Tokens
{
    public interface ITokenService
    {
        public Task<string> GenerateJwtToken(AppUser user);
        public Task<IEnumerable<Claim>> GetClaimsAsync(string jwtToken);

        public Task<string> GetUserIdFromRequest(HttpContext context);
    }
}
