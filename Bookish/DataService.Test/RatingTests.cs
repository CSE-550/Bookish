using Bookish.Data;
using Bookish.DataServices;
using Bookish.Models;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Bookish.DataService.Test
{
    [TestFixture]
    public class RatingTests
    {
        /// <summary>
        /// In memory database context for testing
        /// </summary>
        private Context context;
        /// <summary>
        /// The post service for creating ratings
        /// </summary>
        private RatingService ratingService;
        /// <summary>
        /// The post service for CRUD
        /// </summary>
        private PostService postService;
        /// <summary>
        /// The open library service for additional information
        /// </summary>
        private OpenLibraryService openLibraryService;
        /// <summary>
        /// The comment service for CRUD
        /// </summary>
        private CommentService commentService;
        /// <summary>
        /// The user generating the post
        /// </summary>
        private AuthUserModel authUser;
        /// <summary>
        /// Post Model for rating
        /// </summary>
        private PostModel postModel;
        /// <summary>
        /// Comment model for rating
        /// </summary>
        private CommentModel commentModel;

        [SetUp]
        public async Task Init()
        {
            DbContextOptions<Context> options = new DbContextOptionsBuilder<Context>()
                      .UseInMemoryDatabase("bookish")
                      .Options;
            context = new Context(options);
            commentService = new CommentService(context, new MessageService(context));
            postService = new PostService(context, commentService);
            ratingService = new RatingService(context);
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

            postModel = new PostModel
            {
                Title = "This is a new book",
                Body = "The body of the post",
                Posted_At = DateTime.Now,
                ISBN = "9780553573404",
                Posted_By = authUser.Username
            };

            postModel = await postService.CreatePost(authUser, postModel, openLibraryService);

            commentModel = new CommentModel
            {
                Post_Id = postModel.Id,
                Body = "This is a comment on a post"
            };

            commentModel = commentService.CreateComment(authUser, commentModel);
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

        [TestCase]
        public void TestRatePost_IsUpvote()
        {
            RatingModel rating = ratingService.RatePost(authUser, postModel.Id, true);
            Assert.IsTrue(rating.isUpvote);
        }

        [TestCase]
        public void TestRatePost_IsDownvote()
        {

            RatingModel rating = ratingService.RatePost(authUser, postModel.Id, false);
            Assert.IsFalse(rating.isUpvote);
        }

        [TestCase]
        public void TestRatePost_MissingUser()
        {
            RatingModel rating = ratingService.RatePost(null, postModel.Id, true);
            Assert.IsNull(rating);
        }

        [TestCase]
        public void TestRatePost_Failures()
        {
            Assert.Throws<Exception>(() =>
            {
                RatingModel rating = ratingService.RatePost(authUser, -100, true);
            });
            Assert.Throws<Exception>(() =>
            {
                RatingModel rating = ratingService.RatePost(authUser, postModel.Id, true);
                rating = ratingService.RatePost(authUser, postModel.Id, false);
            });
        }

        [TestCase]
        public void TestRateCommnet_IsUpvote()
        {
            RatingModel rating = ratingService.RateComment(authUser, commentModel.Id, true);
            Assert.IsTrue(rating.isUpvote);
        }

        [TestCase]
        public void TestRateCommnet_IsDownvote()
        {
            RatingModel rating = ratingService.RateComment(authUser, commentModel.Id, false);
            Assert.IsFalse(rating.isUpvote);
        }
        

        [TestCase]
        public void TestRateCommnet_Failures()
        {
            Assert.Throws<Exception>(() =>
            {
                RatingModel rating = ratingService.RateComment(authUser, -100, true);
            });
        }
    }
}
