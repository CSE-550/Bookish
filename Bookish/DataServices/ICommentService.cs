using Bookish.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bookish.DataServices
{
    /// <summary>
    /// ICommentService handles all the CRUD operations
    /// of the comments
    /// </summary>
    public interface ICommentService
    {
        CommentModel GetPostComments(int postId, int skip, int take);

        CommentModel GetSubComments(int commentId, int skip, int take);

        CommentModel CreateComment(CommentModel comment);
    }
}
