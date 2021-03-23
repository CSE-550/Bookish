using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Bookish.Data
{
    public class Rating
    {
        public int Id { get; set; }
        
        public int? Post_Id { get; set; }
        [ForeignKey("Post_Id")]
        public Post Post { get; set; }

        public int? Comment_Id { get; set; }
        [ForeignKey("Comment_Id")]
        public Comment Comment { get; set; }

        public int User_Id { get; set; }
        [ForeignKey("User_Id")]
        public User User { get; set; }

        public bool IsUpvoted { get; set; }
    }
}
