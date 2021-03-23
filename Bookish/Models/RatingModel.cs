using System;
using System.Collections.Generic;
using System.Text;

namespace Bookish.Models
{
    public class RatingModel
    {
        public int Id { get; set; }
        public int? Comment_Id { get; set; }
        public int? Post_Id { get; set; }
        public bool isUpvote { get; set; }
    }
}
