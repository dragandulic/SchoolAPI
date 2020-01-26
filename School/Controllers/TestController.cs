using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using School.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace School.Controllers
{
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TestController : Controller
    {
        private readonly SchoolContext _context;

        public TestController(SchoolContext context)
        {
            _context = context;
        }

        [HttpGet("/api/test")]
        public IActionResult Get()
        {
            //dev branch
            //Change test controller on dev

            var school = new Academy();

            school.Location = "Novi Sad";
            school.Name = "Elektrotehnicka skola Mihajo Pupin";
            school.Description = "Najbolji nacin da predvidite buducnost je da je vi stvarate";

            try
            {
                var res = _context.Academy.Add(school);
                _context.SaveChanges();
                return Ok(new { name = "School is created successfuly !" });
            }
            catch (Exception ex)
            {
                return BadRequest("Something went wrong!");
            }
        }
    }
}
