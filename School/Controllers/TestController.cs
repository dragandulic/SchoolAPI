﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace School.Controllers
{
    public class TestController : Controller
    {

        [HttpGet("api/test")]
        public IActionResult Get()
        {
            return Ok(new { name = "Hello from API !" });
        }
    }
}
