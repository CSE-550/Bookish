using Bookish.Data;
using Bookish.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Mail;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Bookish.DataServices
{
    public class AuthenticationService : IAuthenticationService
    {
        private Context context { get; set; }

        private ICommentService commentService { get; set; }

        private IPostService postService { get; set; }

        public AuthenticationService(Context context, ICommentService commentService, IPostService postService)
        {
            this.context = context;
            this.commentService = commentService;
            this.postService = postService;
        }

        /// <summary>
        /// Creates a hashed password given a password and the salt
        /// </summary>
        /// <param name="password">The password to hash</param>
        /// <param name="salt">The salt to get hash the password with</param>
        /// <returns>
        /// A hashed password
        /// </returns>
        private string HashPassword(string password, byte[] salt) 
        { 
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8)
             );
        }

        /// <summary>
        /// Logins the given user if the username and password match
        /// </summary>
        /// <param name="loginModel">The login model to login</param>
        /// <returns>
        /// The authorized user model
        /// </returns>
        public AuthUserModel Login(UserLoginModel loginModel)
        {
            User user = context.Users
                .Where(u => u.Username == loginModel.Username)
                .FirstOrDefault();

            if (user == null)
            {
                throw new Exception("User not found");
            }

            string hashed = HashPassword(loginModel.Password, user.Salt);

            if (!user.Password.Equals(hashed))
            {
                throw new Exception("Password is incorrect");
            }

            return new AuthUserModel
            {
                Id = user.Id,
                Email = user.Email,
                Username = user.Username,
                // Token is generated server side
                Token = ""
            };
        }

        public void Logout()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates a new user
        /// </summary>
        /// <param name="signUpModel">The user to sign up</param>
        public void SignUp(UserSignUpModel signUpModel)
        {
            // Validate email
            MailAddress addr = new MailAddress(signUpModel.Email);
            if (addr.Address != signUpModel.Email)
            {
                throw new Exception("Email is invalid");
            }

            // TODO: Validate password

            // Validate email and username are not in user
            User foundUser = context.Users
                .Where(u => u.Email == signUpModel.Email || u.Username == signUpModel.Username)
                .FirstOrDefault();

            if (foundUser != null)
            {
                throw new Exception("Username or email already exist");
            }

            // Generate salt
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            User user = new User
            {
                Email = signUpModel.Email,
                Username = signUpModel.Username,
                Password = HashPassword(signUpModel.Password, salt),
                Salt = salt
            };

            context.Users.Add(user);
            context.SaveChanges();
        }

        /// <summary>
        /// Generates an account history of posts and comments
        /// </summary>
        /// <param name="authUser">The authorized user to get the history for</param>
        /// <param name="page">The page number of viewing</param>
        /// <returns>
        /// A list of history items
        /// </returns>
        public HistoryModel History(AuthUserModel authUser, int page)
        {
            // Get a query of the ids in order and the types
            var historyIds = context.Posts
                .Where(p => p.Posted_ById == authUser.Id)
                .Select(p => new
                {
                    Id = p.Id,
                    Created = p.Posted_At,
                    Post = true,
                    Comment = false
                })
                .Union(
                    context.Comments
                        .Where(com => com.Commented_ById == authUser.Id)
                        .Select(com => new
                        {
                            Id = com.Id,
                            Created = com.Commented_At,
                            Post = false,
                            Comment = true
                        })
                )
                .OrderByDescending(item => item.Created)
                .Skip((page-1) * 25)
                .Take(25)
                .ToList();

            // Get the comment ids we need to convert to models
            List<int> commentIds = historyIds.Where(h => h.Comment).Select(h => h.Id).ToList();

            // Setup the query to get the comments
            IQueryable<Comment> commentQuery = context.Comments
                .Where(com => commentIds.Contains(com.Id));

            // Get the comments
            List<CommentModel> comments = commentService.GetCommentModels(commentQuery, authUser.Id);

            // Get the post ids we need to convert to models
            List<int> postIds = historyIds.Where(h => h.Post).Select(h => h.Id).ToList();

            // Setup the query to get the comments
            IQueryable<Post> postQuery = context.Posts
                .Where(p => postIds.Contains(p.Id));

            // Get the posts
            List<PostListModel> posts = postService.GetPostListModels(postQuery, authUser.Id);

            // Setup the results
            return new HistoryModel
            {
                Posts = posts,
                Comments = comments
            };
        }
    }
}
