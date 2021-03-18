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

        public AuthenticationService(Context context)
        {
            this.context = context;
        }

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
                Username = user.Username,
                // Token is generated server side
                Token = ""
            };
        }

        public void Logout()
        {
            throw new NotImplementedException();
        }

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
    }
}
