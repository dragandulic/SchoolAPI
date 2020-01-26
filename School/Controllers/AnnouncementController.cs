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
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AnnouncementController: Controller
    {
        private readonly IAnnouncementService _announcementService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SchoolContext _schoolContext;

        public AnnouncementController(IAnnouncementService announcementService, UserManager<ApplicationUser> userManager,
            SchoolContext schoolContext)
        {
            _announcementService = announcementService;
            _userManager = userManager;
            _schoolContext = schoolContext;
        }

        [HttpGet("api/GetAll")]
        public IActionResult GetAll()
        {
            return Json(_schoolContext.Announcement.ToList());
        }

        [HttpGet("api/GetAnnouncementFromMyClass")]
        public async Task<IActionResult> GetAllFromClass()
        {
            var user = await _userManager.FindByNameAsync(_userManager.GetUserId(HttpContext.User));
            var response = _announcementService.GetAll((long)user.PersonId);

            return Json(response.Value);
        }
    }
}
