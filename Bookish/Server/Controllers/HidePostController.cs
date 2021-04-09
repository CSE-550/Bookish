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
    public class HidePostController : ControllerBase
    {
        /// <summary>
        /// Hides a post
        /// </summary>
        /// <param name="postService">The post service for hiding</param>
        /// <param name="postId">The id of the post being hidden</param>
        /// <returns>
        /// A newly hidden post
        /// </returns>
        [Authorize]
        [HttpGet]
        public PostModel Get([FromServices] IPostService postService, [FromQuery]int postId, [FromQuery]bool hidePost)
        {
            AuthUserModel authUser = (AuthUserModel)this.HttpContext.Items["authUserModel"];
            return postService.HidePost(authUser, postId, hidePost);
        }
    }
}
