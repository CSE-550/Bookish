using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Bookish.Models
{
    /// <summary>
    /// The PostModel is the full post model
    /// in order to display the post for a full
    /// viewing
    /// </summary>
    public class PostModel : PostListModel
    {
        /// <summary>
        /// The body of the post
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// The top level comments on the post
        /// </summary>
        public List<CommentModel> Comments { get; set; }

    }
}
