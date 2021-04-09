using Bookish.DataServices;
using Bookish.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Bookish.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration configuration;

        public LoginController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        /// <summary>
        /// Logins a given user from the login model
        /// </summary>
        /// <param name="authService">The auth service for logging in the user</param>
        /// <param name="userLoginModel">The login model with username and password</param>
        /// <returns>
        /// IActionResult of the auth user that logged in
        /// </returns>
        [HttpPost]
        public IActionResult Post([FromServices] IAuthenticationService authService, UserLoginModel userLoginModel)
        {
            try {
                AuthUserModel authUser = authService.Login(userLoginModel);

                // Get the token settings for generating the auth token
                IConfigurationSection jwtSettings = configuration.GetSection("JWTSettings");
                byte[] key = Encoding.UTF8.GetBytes(jwtSettings.GetSection("securityKey").Value); 
                SigningCredentials creds = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256); 

                // Generates the JWT
                JwtSecurityToken tokenOptions = new JwtSecurityToken(
                    issuer: jwtSettings.GetSection("validIssuer").Value, 
                    audience: jwtSettings.GetSection("validAudience").Value, 
                    claims: new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, authUser.Username),
                        new Claim(ClaimTypes.Email, authUser.Email),
                        new Claim(ClaimTypes.NameIdentifier, authUser.Id.ToString()),
                        new Claim(ClaimTypes.Role, authUser.IsModerator.ToString())
                    }, 
                    expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings.GetSection("expiryInMinutes").Value)), 
                    signingCredentials: creds
                    );
                // Write the JWT
                authUser.Token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
                // Return the user
                return Ok(authUser);
            } 
            catch
            {
                // Failed to login return 401
                return Unauthorized("Username or password is invalid");
            }
        }
    }
}
