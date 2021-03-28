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
    public class PostServiceTests
    {
        /// <summary>
        /// The in memory context
        /// </summary>
        private Context context;
        /// <summary>
        /// The post service for CRUD
        /// </summary>
        private PostService postService;
        /// <summary>
        /// The comment service for CRUD
        /// </summary>
        private CommentService commentService;
        /// <summary>
        /// The open library service for additional information
        /// </summary>
        private OpenLibraryService openLibraryService;
        /// <summary>
        /// The user generating the post
        /// </summary>
        private AuthUserModel authUser;

        /// <summary>
        /// Init the test by creating services and context
        /// </summary>
        [SetUp]
        public void Init()
        {
            DbContextOptions<Context> options = new DbContextOptionsBuilder<Context>()
                      .UseInMemoryDatabase("bookish")
                      .Options;
            context = new Context(options);
            commentService = new CommentService(context);
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
        /// Create a post and verify the information
        /// </summary>
        [TestCase]
        public async Task CreatePost()
        {
            PostModel model = new PostModel
            {
                Title = "This is a new book",
                Body = "The body of the post",
                Posted_At = DateTime.Now,
                ISBN = "9780553573404",
                Posted_By = authUser.Username
            };

            await postService.CreatePost(authUser, model, openLibraryService);

            Post post = context.Posts.FirstOrDefault();

            Assert.AreEqual(model.Title, post.Title);
            Assert.AreEqual(model.Body, post.Body);
            Assert.AreEqual(model.Posted_By, post.Posted_By.Username);
        }

        /// <summary>
        /// Create a post then read the post
        /// </summary>
        [TestCase]
        public async Task ReadPost()
        {
            // Create the post
            await postService.CreatePost(authUser, new PostModel
            {
                Title = "This is a new book",
                Body = "The body of the post",
                Posted_At = DateTime.Now,
                ISBN = "9780553573404",
                Posted_By = authUser.Username
            }, openLibraryService);

            PostModel model = postService.GetPost(1, null);
            Post post = context.Posts.FirstOrDefault();

            Assert.AreEqual(model.Title, post.Title);
            Assert.AreEqual(model.Body, post.Body);
        }

        /// <summary>
        /// Get a list of posts and verify that we get the amount
        /// expected
        /// </summary>
        [TestCase]
        public void ReadPosts()
        {
            List<PostListModel> posts = postService.GetPosts(1, 10, "votes", null);
            Assert.IsTrue(posts.Count() < 10);
        }

    }
}
