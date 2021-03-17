using Bookish.DataServices;
using Bookish.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bookish.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SignUpController : ControllerBase
    {
        /// <summary>
        /// Creates a new user from the signup model if the
        /// model is unique
        /// </summary>
        /// <param name="authService">The auth service for creating the new user</param>
        /// <param name="signUpModel">The sign up model</param>
        /// <returns>
        /// An IActionResult of the success of failure of generating the user
        /// </returns>
        [HttpPut]
        public IActionResult Put([FromServices] IAuthenticationService authService, [FromBody] UserSignUpModel signUpModel)
        {
            try
            {
                authService.SignUp(signUpModel);
                return Ok();
            } 
            catch
            {
                return BadRequest();
            }
        }
    }
}
