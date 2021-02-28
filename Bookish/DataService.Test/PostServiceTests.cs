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
        private Context context;
        private PostService postService;
        private CommentService commentService;

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

        [TestCase]
        public void ReadPost()
        {
            PostModel model = postService.GetPost(1);
            Post post = context.Posts.FirstOrDefault();

            Assert.AreEqual(model.Title, post.Title);
            Assert.AreEqual(model.Body, post.Body);
        }

        [TestCase]
        public void ReadPosts()
        {
            List<PostListModel> posts = postService.GetPosts(1, 10, "votes");
            Assert.AreEqual(1, posts.Count());
        }

    }
}
