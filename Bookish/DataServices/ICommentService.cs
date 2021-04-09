using Bookish.Data;
using Bookish.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bookish.DataServices
{
    /// <summary>
    /// ICommentService handles all the CRUD operations
    /// of the comments
    /// </summary>
    public interface ICommentService
    {
        List<CommentModel> GetPostComments(int postId, int skip, int take, int? userId);

        List<CommentModel> GetSubComments(int commentId, int skip, int take, int? userId);

        CommentModel CreateComment(AuthUserModel authUser, CommentModel comment);

        List<CommentModel> GetCommentModels(IQueryable<Comment> commentQuery, int? userId);

        CommentModel HideComment(AuthUserModel authUser, int commentId, bool hideComment);
    }
}
