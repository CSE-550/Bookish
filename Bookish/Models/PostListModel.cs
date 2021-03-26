using System;
using System.Collections.Generic;
using System.Text;

namespace Bookish.Models
{
    public class PostListModel : IListItem
    {
        /// <summary>
        /// The id of the post
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The post title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// When the post was created
        /// </summary>
        public DateTime Posted_At { get; set; }

        /// <summary>
        /// The total count of comments on the post
        /// </summary>
        public int TotalComments { get; set; }

        /// <summary>
        /// The total number of upvotes of the current post
        /// </summary>
        public int Votes { get; set; }

        /// <summary>
        /// If the current authorized users rating if they
        /// have rated it before
        /// </summary>
        public RatingModel Rating { get; set; }
        
        /// <summary>
        /// The username of the user who created the post
        /// </summary>
        public string Posted_By { get; set; }

        public DateTime GetCreatedDate()
        {
            return Posted_At;
        }
    }
}
