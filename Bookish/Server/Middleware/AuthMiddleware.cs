using Bookish.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Bookish.Server.Middleware
{
    public class AuthMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext httpContext)
        {
            try
            {
                string authToken = httpContext.Request.Headers["Authorization"];
                if (authToken != null)
                {
                    JwtSecurityToken token = new JwtSecurityTokenHandler().ReadJwtToken(authToken.Split(' ')[1]);
                    // Parse out authuser model
                    AuthUserModel model = new AuthUserModel
                    {
                        Id = int.Parse(token.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault()?.Value),
                        Email = token.Claims.Where(c => c.Type == ClaimTypes.Email).FirstOrDefault().Value,
                        Username = token.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value,
                        IsModerator = bool.Parse(token.Claims.Where(c => c.Type == ClaimTypes.Role).FirstOrDefault()?.Value)
                    };
                    httpContext.Items["authUserModel"] = model;
                }

                return _next(httpContext);
            } 
            catch
            {
                return _next(httpContext);
            }
        }
    }
}
