using Bookish.Data;
using Bookish.DataServices;
using Bookish.Models;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bookish.DataService.Test
{
    [TestFixture]
    public class AuthenticationTests
    {
        /// <summary>
        /// The context in memory
        /// </summary>
        private Context context;
        /// <summary>
        /// The auth service for auth CRUD
        /// </summary>
        private AuthenticationService authService;
        /// <summary>
        /// The authorized user
        /// </summary>
        private AuthUserModel authUser;

        /// <summary>
        /// Setup the database and services
        /// </summary>
        [SetUp]
        public void Init()
        {
            DbContextOptions<Context> options = new DbContextOptionsBuilder<Context>()
                      .UseInMemoryDatabase("bookish")
                      .Options;
            context = new Context(options);
            CommentService commentService = new CommentService(context);
            PostService postService = new PostService(context, commentService);
            authService = new AuthenticationService(context, commentService, postService);
            authUser = new AuthUserModel
            {
                Id = 1,
                Username = "ryanenglish"
            };
            context.Users.Add(new User { 
                Id = authUser.Id,
                Username = authUser.Username
            });

            context.SaveChanges();
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
        /// Creates a comment and a post and verifies the 
        /// history amount and the order of the items
        /// </summary>
        [TestCase]
        public void TestHistory()
        {
            // Add a post
            context.Posts.Add(new Post
            {
                Id = 1,
                Posted_At = DateTime.Now,
                Posted_ById = authUser.Id
            });

            // Add a comment
            context.Comments.Add(new Comment
            {
                Id = 1,
                Commented_ById = authUser.Id,
                Commented_At = DateTime.Now
            });

            context.SaveChanges();

            // Get the history
            List<HistoryModel> historyItems = authService.History(authUser, 1);

            Assert.AreEqual(2, historyItems.Count);
            Assert.IsTrue(historyItems[0].IsComment);
            Assert.IsFalse(historyItems[1].IsComment);
        }
    }
}
