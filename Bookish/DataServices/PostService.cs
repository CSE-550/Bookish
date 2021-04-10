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

            if (countPerPage > 100)
            {
                throw new Exception("Pulling out to many posts");
            }

            bool isModerator = authUser?.IsModerator ?? false;

            IQueryable<PostListModel> postQuery = GetPostListModelsQuery(
                context.Posts
                    .Where(p => isModerator || !p.IsHidden)
                    .Skip(skip)
                    .Take(countPerPage),
                authUser?.Id
            );

            switch (orderBy)
            {
                case "votesdesc":
                    postQuery = postQuery.OrderByDescending(post => post.Votes);
                    break;
                case "votesasc":
                    postQuery = postQuery.OrderBy(post => post.Votes);
                    break;
                case "commentsasc":
                    postQuery = postQuery.OrderBy(post => post.TotalComments);
                    break;
                case "commentsdesc":
                    postQuery = postQuery.OrderByDescending(post => post.TotalComments);
                    break;
                case "postdatesasc":
                    postQuery = postQuery.OrderBy(post => post.Posted_At);
                    break;
                case "postdatedesc":
                    postQuery = postQuery.OrderByDescending(post => post.Posted_At);
                    break;
                default:
                    postQuery = postQuery
                        .OrderByDescending(post => post.Votes)
                        .ThenByDescending(post => post.TotalComments)
                        .ThenByDescending(post => post.Posted_At);
                    break;
            }

            return postQuery
                .ToList();
        }

        /// <summary>
        /// Gets a queryable of postlistmodels from a queryable
        /// </summary>
        /// <param name="postQuery">The post query to convert</param>
        /// <returns>
        /// A list of post list models
        /// </returns>
        private IQueryable<PostListModel> GetPostListModelsQuery(IQueryable<Post> postQuery, int? authUserId = null) 
        {
            return postQuery
                .Select(post => new PostListModel
                {
                    Id = post.Id,
                    Posted_At = post.Posted_At,
                    Title = post.Title,
                    Votes = post.Ratings.Where(r => r.IsUpvoted).Count(),
                    Posted_By = post.Posted_By.Username,
                    IsHidden = post.IsHidden,
                    BookTitle = post.BookTitle,
                    Author = post.Author,
                    ISBN = post.ISBN,
                    Rating = authUserId == null ? null : post.Ratings.Where(r => r.User_Id == authUserId).Select(r => new RatingModel
                    {
                        Id = r.Id,
                        Post_Id = r.Post_Id,
                        isUpvote = r.IsUpvoted
                    }).FirstOrDefault(),
                    TotalComments = context.Comments.Where(com => com.Commented_On.Id == post.Id).Count()
                });
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
                    IsHidden = post.IsHidden,
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

            // Determine how to handle author
            string author = null;

            if (opBook == null)
            {
                throw new Exception("Failed to find book information");
            }
            else
            {

                if (opBook.by_statement == null && opBook.authors != null)
                {
                    // Get the author statement
                    OpenLibraryAuthor opauthor = await openLibraryService.GetAuthorInformation(opBook.authors.FirstOrDefault().key);
                    if (opauthor != null)
                    {
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

        /// <summary>
        /// Hides a post if the user is a moderator
        /// </summary>
        /// <param name="authUser">The auth user hiding the post</param>
        /// <param name="postId">The post id</param>
        /// <param name="hidePost">Hides/Unhides the post</param>
        /// <returns>
        /// A newly hidden post
        /// </returns>
        public PostModel HidePost(AuthUserModel authUser, int postId, bool hidePost)
        {
            bool isModerator = context.Users
                .Where(u => u.Id == authUser.Id)
                .Select(u => u.IsModerator)
                .FirstOrDefault();

            if (!isModerator)
            {
                throw new Exception("Not a moderator");
            }

            Post post = context.Posts
                .Where(p => p.Id == postId)
                .FirstOrDefault();

            post.IsHidden = hidePost;
            context.SaveChanges();

            return this.GetPost(post.Id, authUser);
        }

        public List<PostListModel> GetPostListModels(IQueryable<Post> postQuery, int? authUserId)
        {
            return GetPostListModelsQuery(postQuery, authUserId).ToList();
        }
    }
}
