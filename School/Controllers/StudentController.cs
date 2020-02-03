using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using School.Data;
using School.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace School.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("Student")]
    public class StudentController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IStudentService _studentService;

        public StudentController(UserManager<ApplicationUser> userManager, IStudentService studentService)
        {
            _userManager = userManager;
            _studentService = studentService;
        }

        [HttpGet("GetStudentsFromMyClass")]
        public async Task<IActionResult> GetStudentsFromMyClass()
        {
            var user = await _userManager.FindByNameAsync(_userManager.GetUserId(HttpContext.User));

            var users = _userManager.Users.ToList();
            var teachers = new List<long>();
            foreach (var u in users)
            {
                var roles = await _userManager.GetRolesAsync(u);
                foreach (var role in roles)
                {
                    if (role.Equals("Teacher"))
                    {
                        teachers.Add((long)u.PersonId);
                    }
                }
            }


            var response = _studentService.GetStudentsFromMyClass(user.PersonId, teachers);

            if (response.Status == "Success")
            {
                return Json(response.Value);
            }

            return Json(response.Status);

        }


    }
}
