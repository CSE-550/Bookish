using Bookish.DataServices;
using Bookish.Models;
using Microsoft.AspNetCore.Authorization;
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
    public class HistoryController : ControllerBase
    {
        [HttpGet]
        [Authorize]
        public IActionResult Get([FromServices]IAuthenticationService authService, [FromQuery] int page)
        {
            AuthUserModel authUser = (AuthUserModel)this.HttpContext.Items["authUserModel"];
            return Ok(authService.History(authUser, page));
        } 
    }
}
