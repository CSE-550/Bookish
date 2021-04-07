using System;
using System.Collections.Generic;
using System.Text;

namespace Bookish.Models
{
    public class AuthUserModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Username { get; set; } 
        public string Token { get; set; }
        public bool IsModerator { get; set; }
    }
}
