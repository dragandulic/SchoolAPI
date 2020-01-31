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
    public class SchoolController: Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ISchoolService _schoolService;

        public SchoolController(UserManager<ApplicationUser> userManager, ISchoolService schoolService)
        {
            _userManager = userManager;
            _schoolService = schoolService;
        }

        [HttpGet("api/GetSchool")]
        public async Task<IActionResult> GetSchool()
        {
            var currentUser = await _userManager.FindByNameAsync(_userManager.GetUserId(HttpContext.User));
            var currentUserId = (long)currentUser.PersonId;
            var response = _schoolService.GetSchool(currentUserId);

            return Json(response.Value);
        }
    }
}
