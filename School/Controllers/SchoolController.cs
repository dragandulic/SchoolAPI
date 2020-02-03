using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using School.Contracts.Requests;
using School.Data;
using School.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace School.Controllers
{
    public class SchoolController: Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ISchoolService _schoolService;
        private readonly SchoolContext _context;

        public SchoolController(UserManager<ApplicationUser> userManager, ISchoolService schoolService,SchoolContext context)
        {
            _userManager = userManager;
            _schoolService = schoolService;
            _context = context;
        }

        [HttpPost("api/GetSchool")]
        public async Task<IActionResult> GetSchool([FromBody]InitSchoolRequest request)
        {
            Console.WriteLine(request.DeviceToken);
            var currentUser = await _userManager.FindByNameAsync(_userManager.GetUserId(HttpContext.User));
            var currentUserId = (long)currentUser.PersonId;
            var person = _context.Person.Where(c => c.Id == currentUserId).FirstOrDefault();
            person.DeviceId = request.DeviceToken;
            _context.SaveChanges();
            var response = _schoolService.GetSchool(currentUserId);

            return Json(response.Value);
        }
    }
}
