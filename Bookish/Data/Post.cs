using System;
using System.Collections.Generic;
using System.Text;

namespace Bookish.Data
{
    public class Post
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Body { get; set; }

        public DateTime Posted_At { get; set; }

        public ICollection<Comment> Comments { get; set; }

    }
}
