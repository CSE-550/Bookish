using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Bookish.Models
{
    public class UserLoginModel
    {
        [Required]
        [StringLength(16, ErrorMessage = "Username too long (16 character limit).")]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
