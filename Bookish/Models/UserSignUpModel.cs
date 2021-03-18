using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Bookish.Models
{
    /// <summary>
    /// The User Sign Up model generates a new user
    /// from the given information. The model inherits
    /// off the login model in order to correctly
    /// generate the user the same information must
    /// be shared between
    /// </summary>
    public class UserSignUpModel : UserLoginModel
    {
        /// <summary>
        /// The email of the user
        /// </summary>
        [Required]
        public string Email { get; set; }

        /// <summary>
        /// The verification of the password before
        /// hitting the api
        /// </summary>
        [Required]
        public string PasswordReEntry { get; set; }
    }
}
