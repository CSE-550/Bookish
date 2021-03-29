using Bookish.Data;
using Bookish.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        /// <param name="authUser">Current authorized user if there is one</param>
        /// <returns>
        /// A list of PostListModels
        /// </returns>
        public List<PostListModel> GetPosts(int page, int countPerPage, string orderBy, AuthUserModel authUser)
        {
            int skip = (page - 1) * countPerPage;

            if (skip < 0)
            {
                throw new Exception("Pagination must be above 1");
            }

            // TODO: Verify counts and order the posts
            return GetPostListModels(
                context.Posts
                    .Skip(skip)
                    .Take(countPerPage),
                authUser?.Id
            );
        }

        /// <summary>
        /// Gets a list of postlistmodels from a queryable
        /// </summary>
        /// <param name="postQuery">The post query to convert</param>
        /// <returns>
        /// A list of post list models
        /// </returns>
        public List<PostListModel> GetPostListModels(IQueryable<Post> postQuery, int? authUserId = null) 
        {
            return postQuery
                .Select(post => new PostListModel { 
                    Id = post.Id,
                    Posted_At = post.Posted_At,
                    Title = post.Title,
                    Votes = post.Ratings.Where(r => r.IsUpvoted).Count(),
                    Posted_By = post.Posted_By.Username,
                    BookTitle = post.BookTitle,
                    Author = post.Author,
                    ISBN = post.ISBN,
                    Rating = authUserId == null ? null : post.Ratings.Where(r => r.User_Id == authUserId).Select(r => new RatingModel { 
                        Id = r.Id,
                        Post_Id = r.Post_Id,
                        isUpvote = r.IsUpvoted
                    }).FirstOrDefault(),
                    TotalComments = context.Comments.Where(com => com.Commented_On.Id == post.Id).Count()
                })
                .ToList();
        }

        /// <summary>
        /// Gets a complete PostModel by Id
        /// </summary>
        /// <param name="id">The id of the post for viewing</param>
        /// <param name="authUser">Current authorized user if there is one</param>
        /// <returns>
        /// A PostModel for viewing
        /// </returns>
        public PostModel GetPost(int id, AuthUserModel authUser)
        {
            PostModel postModel = context.Posts
                .Where(post => post.Id == id)
                .Select(post => new PostModel { 
                    Id = post.Id,
                    Body = post.Body,
                    Posted_By = post.Posted_By.Username,
                    Posted_At = post.Posted_At,
                    Title = post.Title,
                    ISBN = post.ISBN,
                    Author = post.Author,
                    BookTitle = post.BookTitle,
                    Votes = post.Ratings.Where(r => r.IsUpvoted).Count(),
                    Rating = authUser == null ? null : post.Ratings.Where(r => r.User_Id == authUser.Id).Select(r => new RatingModel { 
                        Id = r.Id,
                        Post_Id = r.Post_Id,
                        isUpvote = r.IsUpvoted
                    }).FirstOrDefault(),
                    TotalComments = post.Comments.Count()
                })
                .FirstOrDefault();

            postModel.Comments = commentService.GetPostComments(id, 0, 15, authUser?.Id);

            void GetComments(CommentModel com)
            {
                com.Comments = commentService.GetSubComments(com.Id, 0, 15, authUser?.Id);
                com.Comments.ForEach(subCom => GetComments(subCom));
            }

            postModel.Comments.ForEach(com => GetComments(com));

            return postModel;
        }

        /// <summary>
        /// Creates a post from a model
        /// </summary>
        /// <param name="postModel">The model for creation</param>
        /// <returns>
        /// The newly created post as a PostModel
        /// </returns>
        public async Task<PostModel> CreatePost(AuthUserModel authUser, PostModel postModel, OpenLibraryService openLibraryService)
        {
            // Get the information
            OpenLibraryBook opBook = await openLibraryService.GetBookInformation(postModel.ISBN);

            if (opBook == null)
            {
                throw new Exception("Failed to find book information");
            }

            // Determine how to handle author
            string author = null;

            if (opBook.by_statement == null && opBook.authors != null)
            {
                // Get the author statement
                OpenLibraryAuthor opauthor = await openLibraryService.GetAuthorInformation(opBook.authors.FirstOrDefault().key);
                if (opauthor != null) {
                    author = opauthor.name;
                } 
            } 
            else
            {
                author = opBook.by_statement;
            }

            Post post = new Post
            {
                Title = postModel.Title,
                Body = postModel.Body,
                Posted_At = DateTime.Now,
                Posted_ById = authUser.Id,
                ISBN = postModel.ISBN,
                BookTitle = opBook.title,
                WorksId = opBook.works.FirstOrDefault()?.key.Replace("/works/", ""),
                Author = author
            };

            context.Posts.Add(post);
            context.SaveChanges();

            return this.GetPost(post.Id, null);
        }
    }
}
