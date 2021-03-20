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
    public class CommentController : ControllerBase
    {
        /// <summary>
        /// Gets a list of comments under a given comment
        /// </summary>
        /// <param name="commentService">The comment service</param>
        /// <param name="commentId">The comment id for getting sub comments</param>
        /// <param name="skip">How many to skip</param>
        /// <param name="take">How many to take</param>
        /// <returns>
        /// A list of comments under a post
        /// </returns>
        [HttpGet]
        public List<CommentModel> Get([FromServices] ICommentService commentService, [FromQuery] int commentId, [FromQuery] int skip, [FromQuery] int take)
        {
            return commentService.GetSubComments(commentId, skip, take);
        }

        /// <summary>
        /// Puts a comment into the database
        /// </summary>
        /// <param name="commentService">The comment service for creation</param>
        /// <param name="commentModel">The comment model being created</param>
        /// <returns>
        /// The newly created comment
        /// </returns>
        [Authorize]
        [HttpPut]
        public CommentModel Put([FromServices] ICommentService commentService, [FromBody] CommentModel commentModel)
        {
            AuthUserModel authUser = (AuthUserModel)this.HttpContext.Items["authUserModel"];
            return commentService.CreateComment(authUser, commentModel);
        }
    }
}
