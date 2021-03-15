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

        [HttpPost]
        public IActionResult Post([FromServices] IAuthenticationService authService, UserLoginModel userLoginModel)
        {
            try {
                AuthUserModel authUser = authService.Login(userLoginModel);

                IConfigurationSection jwtSettings = configuration.GetSection("JWTSettings");

                byte[] key = Encoding.UTF8.GetBytes(jwtSettings.GetSection("securityKey").Value); 
                SigningCredentials creds = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256); 

                JwtSecurityToken tokenOptions = new JwtSecurityToken(
                    issuer: jwtSettings.GetSection("validIssuer").Value, 
                    audience: jwtSettings.GetSection("validAudience").Value, 
                    claims: new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, authUser.Username)
                    }, 
                    expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings.GetSection("expiryInMinutes").Value)), 
                    signingCredentials: creds
                    );
                authUser.Token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
                return Ok(authUser);
            } 
            catch
            {
                return Unauthorized("Username or password is invalid");
            }
        }
    }
}
