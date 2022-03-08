using BackEnd.Data.Entities;
using BackEnd.Models.BindingModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using BackEnd.Models;
using BackEnd.Models.DTO;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using BackEnd.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Collections.Generic;

namespace BackEnd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly JWTConfig _jwtConfig;

        public UserController(ILogger<UserController> logger,
                              UserManager<AppUser> userManager,
                              SignInManager<AppUser> signInManager,
                              RoleManager<IdentityRole> roleManager,
                              IOptions<JWTConfig> jwtConfig)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _jwtConfig = jwtConfig.Value;
            _roleManager = roleManager;
        }

        [HttpPost("RegisterUser")]
        public async Task<object> RegisterUser([FromBody] AddUpdateRegisterUserBindingModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.Role))
                {
                    model.Role = "Student";
                }
                if (!await _roleManager.RoleExistsAsync(model.Role))
                {
                    return await Task.FromResult(new ResponseModel(ResponseCode.Error, "Role does not exist", null));
                }
                var user = new AppUser()
                {
                    FullName = model.FullName,
                    Email = model.Email,
                    UserName = model.Email,
                    DateCreated = DateTime.Now,
                    DateModified = DateTime.Now
                };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    var tempUser = await _userManager.FindByEmailAsync(model.Email);
                    await _userManager.AddToRoleAsync(tempUser, model.Role);
                    return await Task.FromResult(new ResponseModel(ResponseCode.OK, "User has been registered", null));
                }
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, "", result.Errors.Select(x => x.Description).ToArray()));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, ex.Message, null));
            }
        }

        [Authorize(Roles ="Admin")]
        [HttpGet("GetAllUsers")]
        public async Task<object> GetAllUsers()
        {
            try
            {
                List<UserDTO> allUserDTO = new List<UserDTO>();
                var users = _userManager.Users.ToList();
                foreach (var user in users)
                {
                    var role = (await _userManager.GetRolesAsync(user)).FirstOrDefault();

                    allUserDTO.Add(new UserDTO(user.FullName, user.Email, user.UserName, user.DateCreated, role));
                }
                return await Task.FromResult(new ResponseModel(ResponseCode.OK, "", allUserDTO));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, ex.Message, null));
            }
        }

        [Authorize(Roles = "User")]
        [HttpGet("GetUser")]
        public async Task<object> GetUser()
        {
            try
            {
                List<UserDTO> allUserDTO = new List<UserDTO>();
                var users = _userManager.Users.ToList();
                foreach (var user in users)
                {
                    var role = (await _userManager.GetRolesAsync(user)).FirstOrDefault();
                    if (role == "Student")
                    {
                        allUserDTO.Add(new UserDTO(user.FullName, user.Email, user.UserName, user.DateCreated, role));
                    }
                }
                return await Task.FromResult(new ResponseModel(ResponseCode.OK, "", allUserDTO));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, ex.Message, null));
            }
        }

        [HttpPost("Login")]
        public async Task<object> Login([FromBody] LoginBindingModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);

                    if (result.Succeeded)
                    {
                        var appUser = await _userManager.FindByEmailAsync(model.Email);
                        var role = (await _userManager.GetRolesAsync(appUser)).FirstOrDefault();
                        var user = new UserDTO(appUser.FullName, appUser.Email, appUser.UserName, appUser.DateCreated, role);
                        user.Token = GenerateToken(appUser, role);

                        return await Task.FromResult(new ResponseModel(ResponseCode.OK, "", user));
                    }
                }

                return await Task.FromResult(new ResponseModel(ResponseCode.Error, "Invalid Email or Password", null));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, ex.Message, null));
            }
        }

        //[Authorize(Roles = "Admin")]
        [HttpPost("AddRole")]
        public async Task<object> AddRole([FromBody] AddRoleBindingModel model)
        {
            try
            {
                if (model == null || model.Role == "")
                {
                    return await Task.FromResult(new ResponseModel(ResponseCode.Error, "parameters missing", null));
                }
                if (await _roleManager.RoleExistsAsync(model.Role))
                {
                    return await Task.FromResult(new ResponseModel(ResponseCode.OK, "Role already exists", null));
                }
                var role = new IdentityRole();
                role.Name = model.Role;
                var result = await _roleManager.CreateAsync(role);
                if (result.Succeeded)
                {
                    return await Task.FromResult(new ResponseModel(ResponseCode.OK, "Role added sucessfully", null));
                }
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, "something went wrong", null));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, ex.Message, null));
            }
        }


        [HttpGet("GetRoles")]
        public async Task<object> GetRoles()
        {
            try
            {
                var roles = _roleManager.Roles.Select(x=>x.Name).ToList();

                return await Task.FromResult(new ResponseModel(ResponseCode.OK, "", roles));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, ex.Message, null));
            }
        }

        private string GenerateToken(AppUser user, string role)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtConfig.Key);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.NameId, user.Id),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.Role, role)
                }),
                Expires = DateTime.UtcNow.AddHours(12),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Audience = _jwtConfig.Audience,
                Issuer = _jwtConfig.Issuer
            };
            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            return jwtTokenHandler.WriteToken(token);
        }
    }
}
