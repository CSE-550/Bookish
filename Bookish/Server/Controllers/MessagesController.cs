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
    public class MessagesController : ControllerBase
    {
        [HttpGet]
        [Authorize]
        public IActionResult Get([FromServices]IMessageService messageService, int page)
        {
            AuthUserModel authUser = (AuthUserModel)this.HttpContext.Items["authUserModel"];
            return Ok(messageService.GetMessages(authUser, page));
        }
    }
}
