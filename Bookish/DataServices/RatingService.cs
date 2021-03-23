using Bookish.Data;
using Bookish.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bookish.DataServices
{
    public class RatingService : IRatingService
    {
        private Context context;

        public RatingService(Context context)
        {
            this.context = context;
        }

        /// <summary>
        /// Creates a new rating for a post by a user
        /// </summary>
        /// <param name="authUser">The current authorized user</param>
        /// <param name="postId">The post the user is rating</param>
        /// <param name="isUpvote">If the user is upvoting the post</param>
        /// <returns>
        /// The newly created rating
        /// </returns>
        public RatingModel RatePost(AuthUserModel authUser, int postId, bool isUpvote)
        {
            if (!context.Posts.Any(p => p.Id == postId))
            {
                throw new Exception("Post does not exist");
            }

            if (context.Ratings.Any(r => r.Post_Id == postId && r.User_Id == authUser.Id))
            {
                throw new Exception("Rating already exists, use the patch");
            }

            Rating rating = new Rating
            {
                Post_Id = postId,
                User_Id = authUser.Id,
                IsUpvoted = isUpvote
            };

            context.Ratings.Add(rating);
            context.SaveChanges();

            return new RatingModel
            {
                Id = rating.Id,
                Post_Id = postId,
                isUpvote = isUpvote,
            };
        }

        public RatingModel RateComment(AuthUserModel authUser, int commentId, bool isUpvote)
        {
            Comment comment = context.Comments
                .Where(com => com.Id == commentId)
                .FirstOrDefault();

            if (comment == null)
            {
                throw new Exception("Comment does not exist");
            }

            Rating rating = new Rating
            {
                Comment_Id = comment.Id,
                User_Id = authUser.Id
            };

            context.Add(rating);
            context.SaveChanges();

            return new RatingModel
            {
                Id = rating.Id,
                Comment_Id = commentId,
                isUpvote = isUpvote,
            };
        }

        public RatingModel RatingPatch(AuthUserModel authUser, RatingModel ratingModel)
        {
            Rating rating = context.Ratings
                .Where(r => r.Id == ratingModel.Id && authUser.Id == r.User_Id)
                .FirstOrDefault();

            if (rating == null)
            {
                throw new Exception("No rating to patch");
            }

            rating.IsUpvoted = ratingModel.isUpvote;
            context.SaveChanges();

            return ratingModel;
        }

        public RatingModel GetPostRating(AuthUserModel authUer, int postId)
        {
            return context.Ratings
                .Where(r => r.Post_Id == postId && r.User_Id == authUer.Id)
                .Select(r => new RatingModel { 
                    Id = r.Id,
                    Post_Id = r.Post_Id,
                    isUpvote = r.IsUpvoted
                })
                .FirstOrDefault();
        }

        public RatingModel GetCommentRating(AuthUserModel authUer, int commentId)
        {
            return context.Ratings
                .Where(r => r.Comment_Id == commentId && r.User_Id == authUer.Id)
                .Select(r => new RatingModel { 
                    Id = r.Id,
                    Comment_Id = commentId,
                    isUpvote = r.IsUpvoted
                })
                .FirstOrDefault();
        }
    }
}
