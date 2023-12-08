using System.IdentityModel.Tokens.Jwt;
using SaleStreets_Back_end.Models;
using SaleStreets_Back_end.Models.Auth;

namespace SaleStreets_Back_end.Services.Authorization
{
    public interface IAuthService
    {
        Task<AuthModel> RegisterAsync(RegisterModel model);
        Task<AuthModel> LoginAsync(LogInModel model); 

        Task<AuthModel> AddToRoleAsync(AddToRoleModel model);


    }
}
