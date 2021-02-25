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
    public class PostCommentController : ControllerBase
    {
        /// <summary>
        /// Gets a list of comments under a given post
        /// </summary>
        /// <param name="commentService">The comment service</param>
        /// <param name="postId">The post id</param>
        /// <param name="skip">How many to skip</param>
        /// <param name="take">How many to take</param>
        /// <returns>
        /// A list of comments under a post
        /// </returns>
        [HttpGet]
        public List<CommentModel> Get([FromServices] ICommentService commentService, [FromQuery] int postId, [FromQuery] int skip, [FromQuery] int take)
        {
            return commentService.GetPostComments(postId, skip, take);
        }
    }
}
