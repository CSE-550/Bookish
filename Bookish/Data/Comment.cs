using System;
using System.Collections.Generic;
using System.Text;

namespace Bookish.Data
{
    public class Comment
    {
        public int Id { get; set; }

        public string Body { get; set; }

        public DateTime Commented_At { get; set; }

        public virtual Post Commneted_On { get; set; }

        public virtual Comment Commented_Under { get; set; }
    }
}
