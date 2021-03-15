using Bookish.Data;
using Bookish.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
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

        public AuthUserModel Login(UserLoginModel loginModel)
        {
            User user = context.Users
                .Where(u => u.Username == loginModel.Username)
                .FirstOrDefault();

            if (user == null)
            {
                throw new Exception("User not found");
            }

             string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: loginModel.Password,
                salt: user.Salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8)
             );

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

    }
}
