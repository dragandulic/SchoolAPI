using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using School.Contracts.Responses;
using School.Data;

namespace School.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class SubjectsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SchoolContext _context;


        public SubjectsController(UserManager<ApplicationUser> userManager, SchoolContext context)
        {
            _userManager = userManager;
            _context = context;

        }

        [HttpGet("api/GetSubjectsForUser")]
        public async Task<IActionResult> GetSubjectsForUser()
        {
            var user = await _userManager.FindByNameAsync(_userManager.GetUserId(HttpContext.User));
            var class_person = _context.ClassPerson.Where(c => c.PersonId == user.PersonId).Select(s => new {s.Mark, s.ClassId, s.PersonId }).Single();
            string grades = class_person.Mark;
            Console.WriteLine(grades);
            var response = new ClassPersonModel();
            response.ClassId = class_person.ClassId;
            response.PersonId = class_person.PersonId;
            response.Grades = grades;
            return  Json(response);

        }
    }
}