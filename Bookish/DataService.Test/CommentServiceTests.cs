﻿using Bookish.Data;
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
        /// Init db and services
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
        /// Create a post and comment on the post
        /// verifying that the comment is under the post
        /// and contains the same information
        /// </summary>
        [TestCase]
        public void CommentOnPost()
        {
            // Create Post
            PostModel model = new PostModel
            {
                Title = "This is a new book",
                Body = "The body of the post",
                Posted_At = DateTime.Now,
            };

            model = postService.CreatePost(model);

            // Add a comment under the post
            CommentModel commentModel = new CommentModel
            {
                Post_Id = model.Id,
                Body = "This is a comment on a post"
            };

            commentModel = commentService.CreateComment(commentModel);

            Comment comment = context.Comments
                .Where(com => com.Id == commentModel.Id)
                .FirstOrDefault();

            // Verify the comment has the correct information
            Assert.AreEqual(commentModel.Body, comment.Body);
            Assert.AreEqual(commentModel.Post_Id, comment.Commented_OnId);
        }

        /// <summary>
        /// Create a sub level comment and verify that
        /// the information matches
        /// </summary>
        [TestCase]
        public void CommentOnComment()
        {
            // Create top level comment, ignoring post
            CommentModel commentModel = new CommentModel
            {
                Body = "This is a comment"
            };

            commentModel = commentService.CreateComment(commentModel);

            // Create sub level comment
            CommentModel subCommentModel = new CommentModel
            {
                Parent_Id = commentModel.Id,
                Body = "This is a sub comment"
            };

            subCommentModel = commentService.CreateComment(subCommentModel);

            Comment comment = context.Comments
                .Where(com => com.Id == subCommentModel.Id)
                .FirstOrDefault();

            // Verify comment
            Assert.AreEqual(subCommentModel.Body, comment.Body);
            Assert.AreEqual(subCommentModel.Parent_Id, comment.Commented_UnderId);
        }

    }
}
