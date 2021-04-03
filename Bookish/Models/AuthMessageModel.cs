using System;
using System.Collections.Generic;
using System.Text;

namespace Bookish.Models
{
    public class AuthMessageModel
    {
        public int Id { get; set; }
        public bool Seen { get; set; }
        public string Title { get; set; }
        public PostListModel Post { get; set; }
        public CommentModel Comment { get; set; }
    }
}
