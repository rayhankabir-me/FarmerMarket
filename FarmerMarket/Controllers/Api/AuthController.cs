using System;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Web.Http;
using FarmerMarket.Models;
using Microsoft.IdentityModel.Tokens;

namespace FarmerMarket.Controllers.Api
{
    public class AuthController : ApiController
    {
        [HttpPost]
        [Route("api/authenticate")]
        public IHttpActionResult Authenticate(User model)
        {
            var role = model.UserName == "admin" ? "Admin" : "User";

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Convert.FromBase64String(ConfigurationManager.AppSettings["jwtSecret"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, model.UserName),
                    new Claim(ClaimTypes.Role, role),
                }),
                Expires = DateTime.UtcNow.AddDays(10),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(new { Token = tokenString });
        }
    }
}