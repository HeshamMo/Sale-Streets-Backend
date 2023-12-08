using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SaleStreets_Back_end.Configurations;
using SaleStreets_Back_end.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SaleStreets_Back_end.Services.Tokens
{
    public class TokenService:ITokenService
    {
        private readonly JWT _jwt;
        private readonly UserManager<AppUser> _userManager;
        public TokenService(IConfiguration configuration, IOptions<JWT> jwt, UserManager<AppUser> userManager)
        {
            _jwt = jwt.Value;
            _userManager = userManager;
        }

        public async Task<string> GenerateJwtToken(AppUser user)
        {
            var key = Encoding.UTF8.GetBytes(_jwt.Secret);

            var securityKey = new SymmetricSecurityKey(key); 

            var signingCredentials = new
                SigningCredentials(securityKey , SecurityAlgorithms.HmacSha256);     

            var userRoles = await _userManager.GetRolesAsync(user); 
            List<Claim> roleClaims = new List<Claim>();

            foreach(string role in userRoles)
            {
                roleClaims.Add(new Claim(ClaimTypes.Role, role)); 
            }



            var jwtToken = new JwtSecurityToken(
                signingCredentials: signingCredentials,
                claims:new Claim[]
                {
                    new Claim("Id",user.Id),
                    new Claim("Email",user.Email) ,
                    new Claim("Name" , user.FirstName)
                }.Union(roleClaims),
                expires:DateTime.UtcNow.AddDays(1)

                );
                

            return new JwtSecurityTokenHandler().WriteToken(jwtToken);
        }

        public async Task<IEnumerable<Claim>> GetClaimsAsync(string jwtToken)
        {

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.ReadJwtToken(jwtToken);

            return token.Claims; 

        }


        public async Task<string> GetUserIdFromRequest(HttpContext context)
        {
            try
            {
            var token = context.Request.Headers["Authorization"][0].Split(' ')[1];
            var claims = await this.GetClaimsAsync(token);
            var id = claims.FirstOrDefault
                ((claim) => { return claim.Type == "Id"; })?.Value ?? null;
            return id; 

            }
            catch (Exception ex)
            {
                return null; 
            }

        }
    }
}
