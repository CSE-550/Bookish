using System;
using System.Collections.Generic;
using System.Text;

namespace Bookish.Models
{
    public class PostListModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public DateTime Posted_At { get; set; }

        public int TotalComments { get; set; }

        public int Votes { get; set; }
    }
}
