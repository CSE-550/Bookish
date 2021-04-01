using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Bookish.Data
{
    public class Message
    {
        public int Id { get; set; }

        public string Title { get; set; }
        
        public int ForUser_Id { get; set; }
        [ForeignKey("ForUser_Id")]
        public User ForUser { get; set; }

        public int AboutComment_Id { get; set; }
        [ForeignKey("AboutComment_Id")]
        public Comment AboutComment { get; set; }
    }
}
