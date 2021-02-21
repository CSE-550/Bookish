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
    public class PostListController : ControllerBase
    {
        [HttpGet]
        public List<PostListModel> Get([FromServices] IPostService postService, [FromQuery] int page, [FromQuery] int countPerPage, [FromQuery] string orderBy)
        {
            return postService.GetPosts(page, countPerPage, orderBy); 
        }

    }
}
