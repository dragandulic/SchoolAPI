using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace School.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TestController : Controller
    {

        [HttpGet("api/test")]
        public IActionResult Get()
        {
            //dev branch
            //Change test controller on dev
            return Ok(new { name = "Hello from API !" });
        }
    }
}
