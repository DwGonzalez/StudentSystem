using BackEnd.Data;
using BackEnd.Data.Entities;
using BackEnd.Enums;
using BackEnd.Models;
using BackEnd.Models.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentController : Controller
    {
        private readonly ILogger<StudentController> _logger;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly JWTConfig _jwtConfig;
        private readonly AppDBContext _dbContext;

        public StudentController(ILogger<StudentController> logger,
                              UserManager<AppUser> userManager,
                              SignInManager<AppUser> signInManager,
                              RoleManager<IdentityRole> roleManager,
                              IOptions<JWTConfig> jwtConfig,
                              AppDBContext dbContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _jwtConfig = jwtConfig.Value;
            _roleManager = roleManager;
            _dbContext = dbContext;
        }

        //[Authorize(Roles = "Admin")]
        [HttpGet("GetAllStudents")]
        public async Task<object> GetAllStudents()
        {
            try
            {
                string roleName = _roleManager.Roles.Single(x => x.Name == "Student").Name;
                var students = _userManager.GetUsersInRoleAsync(roleName).Result.Select(x => new StudentDTO(x.FullName, x.Email, x.UserName, x.DateCreated));

                return await Task.FromResult(new ResponseModel(ResponseCode.OK, "", students));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, ex.Message, null));
            }
        }

        [HttpGet("GetStudent")]
        public async Task<object> GetStudent()
        {
            try
            {
                string roleName = _roleManager.Roles.Single(x => x.Name == "Student").Name;
                var students = _userManager.GetUsersInRoleAsync(roleName).Result.ToList();

                return await Task.FromResult(new ResponseModel(ResponseCode.OK, "", students));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, ex.Message, null));
            }   
        }

        //[HttpGet("GetStudentSubjects")]
        //public async Task<object> GetStudentSubjects(AppUser student)
        //{
        //    try
        //    {
        //        string studentId = student.Id;
        //        var subjects = _dbContext.StudentSubjects.Where(x=>x.StudentId == studentId).ToList();

        //        if (subjects.Count > 0)
        //        {
        //            return await Task.FromResult(new ResponseModel(ResponseCode.OK, "", subjects));
        //        }

        //        return await Task.FromResult(new ResponseModel(ResponseCode.Error, "No subjects for this student", null));
        //    }
        //    catch (Exception ex)
        //    {
        //        return await Task.FromResult(new ResponseModel(ResponseCode.Error, ex.Message, null));
        //    }
        //}
    }
}
