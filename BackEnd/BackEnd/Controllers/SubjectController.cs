using BackEnd.Data;
using BackEnd.Data.Entities;
using BackEnd.Enums;
using BackEnd.Models;
using BackEnd.Models.BindingModel;
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
    public class SubjectController : Controller
    {
        private readonly ILogger<SubjectController> _logger;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly JWTConfig _jwtConfig;
        private readonly AppDBContext _dbContext;

        public SubjectController(ILogger<SubjectController> logger,
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
        [HttpGet("GetAllSubjects")]
        public async Task<object> GetAllSubjects()
        {
            try
            {
                //var subjects = _dbContext.Subjects.ToList();
                var subjects = _dbContext.Subjects.Select(x => new SubjectDTO(x.Id, x.ClassName,
                    new UserDTO(x.Professor.FullName, x.Professor.Email, x.Professor.UserName, x.DateCreated, "Professor"),
                    new RoomDTO(x.Room.RoomName, x.Room.Id)));
                if (subjects != null)
                {
                    return await Task.FromResult(new ResponseModel(ResponseCode.OK, "", subjects));
                }

                return await Task.FromResult(new ResponseModel(ResponseCode.Error, "There are no subjects", null));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, ex.Message, null));
            }
        }

        //[Authorize(Roles = "Admin")]
        [HttpPost("AddSubject")]
        public async Task<object> AddSubject([FromBody] AddUpdateSubjectBindingModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var subjectExists = _dbContext.Subjects.Where(x => x.ClassName == model.SubjectName).ToList();
                    if (subjectExists.Count > 0)
                    {
                        return await Task.FromResult(new ResponseModel(ResponseCode.Error, "Subject already exists", null));
                    }

                    var subject = new Subject()
                    {
                        ClassName = model.SubjectName,
                        RoomId = model.RoomId,
                        ProfessorId = model.ProfessorId
                    };
                    _dbContext.Subjects.Add(subject);
                    _dbContext.SaveChanges();

                    return await Task.FromResult(new ResponseModel(ResponseCode.OK, "Subject has been registered", null));

                }
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, "Parameters missing", null));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, ex.Message, null));
            }
        }


        //[Authorize(Roles = "Student")]
        [HttpGet("GetStudentSubjects")]
        public async Task<object> GetStudentSubjects(string email)
        {
            try
            {
                var appUser = await _userManager.FindByEmailAsync(email);
                string userId = appUser.Id;

                var subjects = _dbContext.StudentSubjects.Where(x => x.StudentId != userId).ToList();
                var subjects2 = _dbContext.StudentSubjects.Select(x => new SubjectStudentDTO(new SubjectDTO(x.Id, x.Subject.ClassName,
                    new UserDTO(x.Subject.Professor.FullName, x.Subject.Professor.Email, x.Subject.Professor.UserName, x.Subject.Professor.DateCreated, "Professor"),
                    new RoomDTO(x.Subject.Room.RoomName, x.Subject.Room.Id)),
                    new StudentDTO(x.Student.FullName, x.Student.Email, x.Student.UserName, x.Student.DateCreated)));

                if (subjects2 != null)
                {
                    return await Task.FromResult(new ResponseModel(ResponseCode.OK, "", subjects2));
                }

                if (subjects.Count > 0)
                {
                    return await Task.FromResult(new ResponseModel(ResponseCode.OK, "", subjects));
                }

                return await Task.FromResult(new ResponseModel(ResponseCode.OK, "No subjects for this student", null));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, ex.Message, null));
            }
        }

        //[Authorize(Roles = "Professor")]
        [HttpGet("GetProfesorSubjects")]
        public async Task<object> GetProffesorSubjects()
        {
            try
            {
                //List<UserDTO> allUserDTO = new List<UserDTO>();
                //var users = _userManager.Users.ToList();
                //foreach (var user in users)
                //{
                //    var role = (await _userManager.GetRolesAsync(user)).FirstOrDefault();

                //    allUserDTO.Add(new UserDTO(user.FullName, user.Email, user.UserName, user.DateCreated, role));
                //}
                return await Task.FromResult(new ResponseModel(ResponseCode.OK, "nothing", null));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, ex.Message, null));
            }
        }
    }
}
