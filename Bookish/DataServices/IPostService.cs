using Bookish.Data;
using Bookish.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bookish.DataServices
{
    /// <summary>
    /// The interface of the posts that stores
    /// all the possible CRUD operations
    /// </summary>
    public interface IPostService
    {
        List<PostListModel> GetPosts(int page, int countPerPage, string orderBy, AuthUserModel authUser);

        PostModel GetPost(int id, AuthUserModel authUser);

        PostModel CreatePost(AuthUserModel authUser, PostModel postModel);

        List<PostListModel> GetPostListModels(IQueryable<Post> postQuery, int? authUserId = null);
    }
}
