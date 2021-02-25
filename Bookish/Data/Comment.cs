using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Bookish.Data
{
    public class Comment
    {
        public int Id { get; set; }

        public string Body { get; set; }

        public DateTime Commented_At { get; set; }

        public int Commented_OnId { get; set; }
        [ForeignKey("Commented_OnId")]
        public virtual Post Commented_On { get; set; }

        public int? Commented_UnderId { get; set; }
        [ForeignKey("Commented_UnderId")]
        public virtual Comment Commented_Under { get; set; }
    }
}
