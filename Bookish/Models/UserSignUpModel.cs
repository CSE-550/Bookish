using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Bookish.Models
{
    public class UserSignUpModel : UserLoginModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string PasswordReEntry { get; set; }
    }
}
