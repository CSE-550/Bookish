using Bookish.Data;
using Bookish.DataServices;
using Bookish.Models;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataService.Test
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
        /// Init the test by creating services and context
        /// </summary>
        [OneTimeSetUp]
        public void Init()
        {
            DbContextOptions<Context> options = new DbContextOptionsBuilder<Context>()
                      .UseInMemoryDatabase("bookish")
                      .Options;
            context = new Context(options);
            commentService = new CommentService(context);
            postService = new PostService(context, commentService);
        }

        /// <summary>
        /// Create a post and verify the information
        /// </summary>
        [TestCase]
        public void CreatePost()
        {
            PostModel model = new PostModel
            {
                Title = "This is a new book",
                Body = "The body of the post",
                Posted_At = DateTime.Now,
            };

            postService.CreatePost(model);

            Post post = context.Posts.FirstOrDefault();

            Assert.AreEqual(model.Title, post.Title);
            Assert.AreEqual(model.Body, post.Body);
        }

        /// <summary>
        /// Read the post created above and verify the model
        /// </summary>
        [TestCase]
        public void ReadPost()
        {
            PostModel model = postService.GetPost(1);
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
            List<PostListModel> posts = postService.GetPosts(1, 10, "votes");
            Assert.IsTrue(posts.Count() < 10);
        }

    }
}
