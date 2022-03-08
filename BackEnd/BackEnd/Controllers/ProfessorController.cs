using BackEnd.Data;
using BackEnd.Data.Entities;
using BackEnd.Enums;
using BackEnd.Models;
using BackEnd.Models.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProfessorController : Controller
    {
        private readonly AppDBContext _dbContext;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public ProfessorController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager ,AppDBContext dbContext)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _dbContext = dbContext;
        }

        //[Authorize(Roles = "Admin")]
        [HttpGet("GetAllProfessors")]
        public async Task<object> GetAllProfessors()
        {
            try
            {
                string roleName = _roleManager.Roles.Single(x => x.Name == "Professor").Name;
                var students = _userManager.GetUsersInRoleAsync(roleName).Result.Select(x => new StudentDTO(x.FullName, x.Email, x.UserName, x.DateCreated));

                return await Task.FromResult(new ResponseModel(ResponseCode.OK, "", students));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, ex.Message, null));
            }
        }
    }
}
