using Bookish.Data;
using Bookish.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bookish.DataServices
{
    /// <summary>
    /// PostService handles the basic Post CRUD operations
    /// </summary>
    public class PostService : IPostService
    {
        private Context context { get; set; }

        private ICommentService commentService { get; set; }

        public PostService(Context context, ICommentService commentService)
        {
            this.context = context;
            this.commentService = commentService;
        }

        /// <summary>
        /// Gets a list of PostListModel models paginated
        /// </summary>
        /// <param name="page">The current page for viewing</param>
        /// <param name="countPerPage">The amount of posts per page</param>
        /// <param name="orderBy">How to order the posts</param>
        /// <returns>
        /// A list of PostListModels
        /// </returns>
        public List<PostListModel> GetPosts(int page, int countPerPage, string orderBy)
        {
            int skip = (page - 1) * countPerPage;

            if (skip < 0)
            {
                throw new Exception("Pagination must be above 1");
            }

            // TODO: Verify counts and order the posts
            return context.Posts
                .Skip(skip)
                .Take(countPerPage)
                .Select(post => new PostListModel { 
                    Id = post.Id,
                    Posted_At = post.Posted_At,
                    Title = post.Title
                })
                .ToList();
        }

        /// <summary>
        /// Gets a complete PostModel by Id
        /// </summary>
        /// <param name="id">The id of the post for viewing</param>
        /// <returns>
        /// A PostModel for viewing
        /// </returns>
        public PostModel GetPost(int id)
        {
            PostModel postModel = context.Posts
                .Where(post => post.Id == id)
                .Select(post => new PostModel { 
                    Id = post.Id,
                    Body = post.Body,
                    Posted_At = post.Posted_At,
                    Title = post.Title,
                    TotalComments = post.Comments.Count()
                })
                .FirstOrDefault();

            postModel.Comments = commentService.GetPostComments(id, 0, 50);

            return postModel;
        }

        /// <summary>
        /// Creates a post from a model
        /// </summary>
        /// <param name="postModel">The model for creation</param>
        /// <returns>
        /// The newly created post as a PostModel
        /// </returns>
        public PostModel CreatePost(PostModel postModel)
        {
            Post post = new Post
            {
                Title = postModel.Title,
                Body = postModel.Body,
                Posted_At = DateTime.Now
            };

            context.Posts.Add(post);
            context.SaveChanges();

            return this.GetPost(post.Id);
        }
    }
}
