using Bookish.Data;
using Bookish.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        Task<PostModel> CreatePost(AuthUserModel authUser, PostModel postModel, OpenLibraryService openLibraryService);

        List<PostListModel> GetPostListModels(IQueryable<Post> postQuery, int? authUserId = null);

        PostModel HidePost(AuthUserModel authUser, int postId, bool hidePost);
    }
}
