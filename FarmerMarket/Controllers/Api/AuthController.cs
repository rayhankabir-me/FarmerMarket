using System;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Sockets;
using System.Security.Claims;
using System.Text;
using System.Web.Http;
using FarmerMarket.Context;
using FarmerMarket.Models;
using Microsoft.IdentityModel.Tokens;

namespace FarmerMarket.Controllers.Api
{
    public class AuthController : ApiController
    {
        private FarmerMarketContext _context = new FarmerMarketContext();

        [HttpPost]
        [Route("api/auth/login")]
        public IHttpActionResult Authenticate([FromBody] User model)
        {


            var user = _context.Users.FirstOrDefault(u => u.UserName == model.UserName);

            if (user == null)
            {
                return BadRequest("Sorry, no user found...!");
            }

            if (user.Password != model.Password)
            {
                return BadRequest("Invalid login credentials...!");
            }

            var role = user.Role;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Convert.FromBase64String(ConfigurationManager.AppSettings["jwtSecret"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Role, role),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim("UserId", user.UserId.ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = ConfigurationManager.AppSettings["jwtIssuer"],
                Audience = ConfigurationManager.AppSettings["jwtAudience"]
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(new { Token = tokenString });
        }

    }

}





    