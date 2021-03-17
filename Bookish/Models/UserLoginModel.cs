using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Bookish.Models
{
    /// <summary>
    /// The user login model is the form of logging
    /// a user, must have username and password
    /// to login to the application
    /// </summary>
    public class UserLoginModel
    {
        /// <summary>
        /// The username for logging in
        /// </summary>
        [Required]
        [StringLength(16, ErrorMessage = "Username too long (16 character limit).")]
        public string Username { get; set; }

        /// <summary>
        /// The password for logging in
        /// </summary>
        [Required]
        public string Password { get; set; }
    }
}
