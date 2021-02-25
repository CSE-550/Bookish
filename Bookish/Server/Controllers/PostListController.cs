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
        /// <summary>
        /// Gets a list of postlistmodels
        /// </summary>
        /// <param name="postService">The postservice for reading the list of posts</param>
        /// <param name="page">The page number being displayed</param>
        /// <param name="countPerPage">The amount of posts to display per page</param>
        /// <param name="orderBy">How the posts are orderd</param>
        /// <returns>
        /// A list of posts
        /// </returns>
        [HttpGet]
        public List<PostListModel> Get([FromServices] IPostService postService, [FromQuery] int page, [FromQuery] int countPerPage, [FromQuery] string orderBy)
        {
            return postService.GetPosts(page, countPerPage, orderBy); 
        }

    }
}
