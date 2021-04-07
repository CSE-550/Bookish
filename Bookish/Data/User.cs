using System;
using System.Collections.Generic;
using System.Text;

namespace Bookish.Data
{
    /// <summary>
    /// An authorized user of the application
    /// </summary>
    public class User
    {
        /// <summary>
        /// The id of the user
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// The email of the user in order to
        /// verify the users account
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// The unique username of a given user
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// The hashed password of the user
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// The salt for generating the password
        /// </summary>
        public byte[] Salt { get; set; }

        /// <summary>
        /// Determines if the user has the ability to hide and unhide posts
        /// </summary>
        public bool IsModerator { get; set; }

        public ICollection<Rating> Ratings { get; set; }
    }
}
