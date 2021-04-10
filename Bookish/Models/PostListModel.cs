using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        /// The ISBN of the book for getting information
        /// </summary>
        [MaxLength(14)]
        [MinLength(10)]
        public string ISBN { get; set; }

        /// <summary>
        /// The post title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The title of the book being discussed
        /// </summary>
        public string BookTitle { get; set; }

        /// <summary>
        /// The name of the author, if known
        /// </summary>
        public string Author { get; set; }

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

        /// <summary>
        /// Determines if a moderator hide a post 
        /// </summary>
        public bool IsHidden { get; set; }

        public DateTime GetCreatedDate()
        {
            return Posted_At;
        }
    }
}
