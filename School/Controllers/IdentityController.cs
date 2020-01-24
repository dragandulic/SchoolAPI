using Microsoft.AspNetCore.Mvc;
using School.Contracts;
using School.Contracts.Requests;
using School.Contracts.Responses;
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

        public IdentityController(IIdentityService identityService)
        {
            _identityService = identityService;
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

    }
}
