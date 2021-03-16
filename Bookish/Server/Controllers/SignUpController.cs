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
