using Bookish.Data;
using Bookish.DataServices;
using Bookish.Models;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookish.DataService.Test
{
    [TestFixture]
    public class CommentServiceTests
    {
        /// <summary>
        /// In memory database context for testing
        /// </summary>
        private Context context;
        /// <summary>
        /// The post service for creating posts
        /// </summary>
        private PostService postService;
        /// <summary>
        /// The comment service for creating and reading comments
        /// </summary>
        private CommentService commentService;
        /// <summary>
        /// The message service for creating messages
        /// </summary>
        private MessageService messageService;
        /// <summary>
        /// The service for making http request to open library
        /// </summary>
        private OpenLibraryService openLibraryService;
        /// <summary>
        /// The user generating the comment
        /// </summary>
        private AuthUserModel authUser;

        /// <summary>
        /// Init db and services
        /// </summary>
        [SetUp]
        public void Init()
        {
            DbContextOptions<Context> options = new DbContextOptionsBuilder<Context>()
                      .UseInMemoryDatabase("bookish")
                      .Options;
            context = new Context(options);
            commentService = new CommentService(context, messageService);
            postService = new PostService(context, commentService);
            openLibraryService = new OpenLibraryService(new System.Net.Http.HttpClient());
            authUser = new AuthUserModel
            {
                Id = 1,
                Username = "ryanenglish"
            };
            context.Users.Add(new User { 
                Id = authUser.Id,
                Username = authUser.Username
            });
        }
        
        /// <summary>
        /// Disposes the database after every test
        /// </summary>
        [TearDown]
        public void Dispose()
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }

        /// <summary>
        /// Create a post and comment on the post
        /// verifying that the comment is under the post
        /// and contains the same information
        /// </summary>
        [TestCase]
        public async Task CommentOnPost()
        {
            // Create Post
            PostModel model = new PostModel
            {
                Title = "This is a new book",
                Body = "The body of the post",
                Posted_At = DateTime.Now,
                ISBN = "9780553573404"
            };

            model = await postService.CreatePost(authUser, model, openLibraryService);

            // Add a comment under the post
            CommentModel commentModel = new CommentModel
            {
                Post_Id = model.Id,
                Body = "This is a comment on a post"
            };

            commentModel = commentService.CreateComment(authUser, commentModel);

            Comment comment = context.Comments
                .Where(com => com.Id == commentModel.Id)
                .FirstOrDefault();

            // Verify the comment has the correct information
            Assert.AreEqual(commentModel.Body, comment.Body);
            Assert.AreEqual(commentModel.Commented_By, comment.Commented_By.Username);
            Assert.AreEqual(commentModel.Post_Id, comment.Commented_OnId);
        }

        /// <summary>
        /// Create a sub level comment and verify that
        /// the information matches
        /// </summary>
        [TestCase]
        public async Task CommentOnComment()
        {
            // Create Post
            PostModel model = new PostModel
            {
                Title = "This is a new book",
                Body = "The body of the post",
                Posted_At = DateTime.Now,
                ISBN = "9780553573404"
            };

            model = await postService.CreatePost(authUser, model, openLibraryService);

            // Create top level comment, ignoring post
            CommentModel commentModel = new CommentModel
            {
                Body = "This is a comment",
                Post_Id = model.Id
            };

            commentModel = commentService.CreateComment(authUser, commentModel);

            // Create sub level comment
            CommentModel subCommentModel = new CommentModel
            {
                Parent_Id = commentModel.Id,
                Body = "This is a sub comment"
            };

            subCommentModel = commentService.CreateComment(authUser, subCommentModel);

            Comment comment = context.Comments
                .Where(com => com.Id == subCommentModel.Id)
                .FirstOrDefault();

            // Verify comment
            Assert.AreEqual(subCommentModel.Body, comment.Body);
            Assert.AreEqual(subCommentModel.Commented_By, comment.Commented_By.Username);
            Assert.AreEqual(subCommentModel.Parent_Id, comment.Commented_UnderId);
        }

    }
}
