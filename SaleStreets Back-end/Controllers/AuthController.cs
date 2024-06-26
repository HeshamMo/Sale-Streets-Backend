﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SaleStreets_Back_end.Models;
using SaleStreets_Back_end.Models.Auth;
using SaleStreets_Back_end.Services.Authorization;
using SaleStreets_Back_end.Services.Tokens;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace SaleStreets_Back_end.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController:ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ITokenService _tokenService;

        public AuthController(IAuthService authService, ITokenService tokenService)
        {
            _authService = authService;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync(RegisterModel model)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(new AuthModel()
                {
                    Success = false,
                    Message = "validation Error"
                });
            }

            AuthModel result = await _authService.RegisterAsync(model);
            if(result.Success)
            {
                _authService.AddAuthCookie(this.Response, result.Token);
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }


        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LogInModel model)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(new AuthModel()
                {
                    Success = false,
                    Message = "validation Error"
                });
            }

            var result = await _authService.LoginAsync(model);
            if(result.Success)
            {
                //     Response.Headers.Authorization = 

                _authService.AddAuthCookie(this.Response, result.Token); 
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }






    }
}
