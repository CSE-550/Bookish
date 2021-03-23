using Bookish.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bookish.DataServices
{
    public interface IRatingService
    {
        RatingModel RatePost(AuthUserModel authUer, int postId, bool isUpvote);
        RatingModel RateComment(AuthUserModel authUer, int commentId, bool isUpvote);
        RatingModel RatingPatch(AuthUserModel authUer, RatingModel ratingModel);
        RatingModel GetPostRating(AuthUserModel authUer, int postId);
        RatingModel GetCommentRating(AuthUserModel authUer, int commentId);
    }
}
