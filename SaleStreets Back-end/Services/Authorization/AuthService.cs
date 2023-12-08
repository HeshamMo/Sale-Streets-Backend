using Microsoft.AspNetCore.Identity;
using SaleStreets_Back_end.Models;
using SaleStreets_Back_end.Models.Auth;
using SaleStreets_Back_end.Services.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace SaleStreets_Back_end.Services.Authorization
{
    public class AuthService:IAuthService
    {
        private UserManager<AppUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;

        private readonly ITokenService _tokenSerice;
        public AuthService(RoleManager<IdentityRole> roleManager, 
            UserManager<AppUser> userManager, ITokenService tokenSerice)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _tokenSerice = tokenSerice;
        }

        public Task<AuthModel> AddToRoleAsync(AddToRoleModel model)
        {
            throw new NotImplementedException();
        }


        public async Task<AuthModel> LoginAsync(LogInModel model)
        {
           var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null || await _userManager.CheckPasswordAsync(user,model.Password))
            {
                return new AuthModel()
                {
                    Success = false , 
                    Message  = "Email or password is incorrect"
                };
            }

            string token = await _tokenSerice.GenerateJwtToken(user);
            var userRoles = await _userManager.GetRolesAsync(user);

            return new AuthModel()
            {
                Success = true,
                Email = model.Email,
                Roles = userRoles.ToList<String>(),
                Token = token,               
            };

        }

        public async Task<AuthModel> RegisterAsync(RegisterModel model)
        {
            if (await _userManager.FindByEmailAsync(model.Email)!=null)
            {
                return new AuthModel() { Success = false, Message = "Email is Already Taken" }; 
            }


            var user = new AppUser()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                Password= model.Password,
                UserName = model.Email.Split('@')[0]
                
                
            }; 

            IdentityResult result = await _userManager.CreateAsync(user);
            if(result.Succeeded)
            {
                var _user = await _userManager.FindByEmailAsync(user.Email);
                await _userManager.AddToRoleAsync(_user, "user");

                return new AuthModel()
                {
                    Success = true,
                    Email = model.Email,
                    Roles = new List<string>() { "user" },
                    Token = await _tokenSerice.GenerateJwtToken(_user)
                }; 
            }
            else
            {
                var errorMsg = ""; 
                foreach(var error in result.Errors)
                {
                    errorMsg += $"{error.Description} ,";
                }
                return new AuthModel()
                {
                    Success = false,
                    Message = errorMsg
                };
            }

        }
    }
}
