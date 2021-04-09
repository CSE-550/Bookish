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
    public class HideCommentController : ControllerBase
    {
        /// <summary>
        /// Hides/Unhides a comment as a moderator
        /// </summary>
        /// <param name="commentService">The comment service for CRUD</param>
        /// <param name="commentId">The id of the comment</param>
        /// <param name="hideComment">Hides/Unhides</param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public CommentModel Get([FromServices] ICommentService commentService, [FromQuery]int commentId, [FromQuery]bool hideComment)
        {
            AuthUserModel authUser = (AuthUserModel)this.HttpContext.Items["authUserModel"];
            return commentService.HideComment(authUser, commentId, hideComment);
        }
    }
}
