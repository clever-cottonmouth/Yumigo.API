﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using Yumigo.API.Models;
using Yumigo.API.Models.DTO;
using Yumigo.API.Utility;

namespace Yumigo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApiResponse _response;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly string _secretKey;

        public AuthController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _secretKey = configuration.GetValue<string>("ApiSetting:Secret")??"";
            _response = new ApiResponse();
            _userManager = userManager;
            _roleManager = roleManager;
        }


        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser newUser = new()
                {
                    Email = model.Email,
                    UserName = model.Email,
                    Name = model.Name,
                    NormalizedEmail = model.Email.ToUpper(),
                };

                var result = await _userManager.CreateAsync(newUser, model.Password);
                if (result.Succeeded) 
                {
                    #region [ROLE]

                    if (!_roleManager.RoleExistsAsync(SD.Role_Admin).GetAwaiter().GetResult())
                    {
                        await _roleManager.CreateAsync(new IdentityRole(SD.Role_Admin));
                        await _roleManager.CreateAsync(new IdentityRole(SD.Role_Customer));
                    }

                    if(model.Role.Equals(SD.Role_Admin, StringComparison.CurrentCultureIgnoreCase))
                    {
                        await _userManager.AddToRoleAsync(newUser, SD.Role_Admin);
                    }
                    else
                    {
                        await _userManager.AddToRoleAsync(newUser, SD.Role_Customer);
                    }
                    #endregion


                    _response.StatusCode = HttpStatusCode.OK;
                    _response.IsSuccess = true;
                    return Ok(_response);
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        _response.ErrorMessages.Add(error.Description);
                    }
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    return BadRequest(_response);
                }
            }
            else
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                foreach(var error in ModelState.Values)
                {
                    foreach(var err in error.Errors)
                    {
                        _response.ErrorMessages.Add(err.ErrorMessage);
                    }
                }
                return BadRequest(_response);
            }
         
        }



        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO model)
        {
            if (ModelState.IsValid)
            {
                var userFromDb = await _userManager.FindByEmailAsync(model.Email);
                if (userFromDb != null)
                {
                    bool isValid = await _userManager.CheckPasswordAsync(userFromDb, model.Password);
                    if (!isValid)
                    {
                        _response.Result = new LoginResponseDTO();
                        _response.StatusCode = HttpStatusCode.BadRequest;
                        _response.IsSuccess = false;
                        _response.ErrorMessages.Add("Invalid Password");
                        return BadRequest(_response);
                    }

                    JwtSecurityTokenHandler tokenHandler = new();
                    byte[] key = Encoding.ASCII.GetBytes(_secretKey);

                    SecurityTokenDescriptor tokenDescriptor = new()
                    {
                        Subject = new ClaimsIdentity(
                            [
                            new ("fullName", userFromDb.Name),
                            new ("id", userFromDb.Id),
                            new (ClaimTypes.Email, userFromDb.Email!.ToString()),
                            new (ClaimTypes.Role, _userManager.GetRolesAsync(userFromDb).Result.FirstOrDefault()!)
                            ]
                            ),
                        Expires = DateTime.UtcNow.AddDays(7),
                        SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                    };

                    SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

                    LoginResponseDTO loginResponse = new()
                    {
                        Email = userFromDb.Email,
                        Token = tokenHandler.WriteToken(token),
                        Role = _userManager.GetRolesAsync(userFromDb).Result.FirstOrDefault() ?? string.Empty
                    };


                    _response.Result = loginResponse;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.IsSuccess = true;
                    return Ok(_response);
                }

                _response.Result = new LoginResponseDTO();
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Invalid credentials");
                return BadRequest(_response);
            }
            else
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                foreach (var error in ModelState.Values)
                {
                    foreach (var err in error.Errors)
                    {
                        _response.ErrorMessages.Add(err.ErrorMessage);
                    }
                }
                return BadRequest(_response);
            }

        }
    }
}
