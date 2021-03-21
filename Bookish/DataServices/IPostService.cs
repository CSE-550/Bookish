using Bookish.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bookish.DataServices
{
    /// <summary>
    /// The interface of the posts that stores
    /// all the possible CRUD operations
    /// </summary>
    public interface IPostService
    {
        List<PostListModel> GetPosts(int page, int countPerPage, string orderBy);

        PostModel GetPost(int id);

        PostModel CreatePost(AuthUserModel authUser, PostModel postModel);
    }
}
