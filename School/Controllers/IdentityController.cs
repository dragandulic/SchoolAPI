using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using School.Contracts;
using School.Contracts.Requests;
using School.Contracts.Responses;
using School.Data;
using School.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace School.Controllers
{
    public class IdentityController : Controller
    {
        private readonly IIdentityService _identityService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public IdentityController(IIdentityService identityService, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _identityService = identityService;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpPost("api/registration")]
        public async Task<IActionResult> Registration( UserRegistrationRequest model)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(new AuthFailedResponse
                {
                    Errors = ModelState.Values.SelectMany(x => x.Errors.Select(xx => xx.ErrorMessage))
                });
            }

            var authResponse = await _identityService.RegisterAsync(model);

            if (!authResponse.Success)
            {
                return BadRequest(new AuthFailedResponse
                {
                    Errors = authResponse.Errors
                });
            }

            return Ok();
        }

        [HttpPost("api/login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest request)
        {

            var authResponse = await _identityService.LoginAsync(request.Email, request.Password);

            if (!authResponse.Success)
            {
                return BadRequest(new AuthFailedResponse
                {
                    Errors = authResponse.Errors
                });
            }

            return Ok(new AuthSuccessResponse
            {
                Token = authResponse.Token
            });
        }

        [HttpPost("api/createRole")]
        public async Task<IActionResult> CreateRole(string roleName)
        {
            var response = await _identityService.CreateRole(roleName);
         
            return Json(response);
        }

        [HttpGet("api/GetPerson")]
        public async Task<IActionResult> GetPerson()
        {
            var user = await _userManager.FindByNameAsync(_userManager.GetUserId(HttpContext.User));
            var person = _identityService.GetPerson((long)user.PersonId).Value;
            person.Email = user.Email;
            return Json(person);
        }

        [HttpGet("api/GetRole")]
        public async Task<IActionResult> GetRole()
        {
            var loggedUser = await _userManager.FindByNameAsync(_userManager.GetUserId(HttpContext.User));
            var roles = await _userManager.GetRolesAsync(loggedUser);
            var r = roles.FirstOrDefault();
            return Json(roles.FirstOrDefault());
        }

    }
}
