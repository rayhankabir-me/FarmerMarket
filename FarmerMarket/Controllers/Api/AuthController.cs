using System;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Sockets;
using System.Security.Claims;
using System.Text;
using System.Web.Http;
using FarmerMarket.Models;
using Microsoft.IdentityModel.Tokens;

namespace FarmerMarket.Controllers.Api
{
    public class AuthController : ApiController
    {
        [HttpPost]
        [Route("api/authenticate")]
        public IHttpActionResult Authenticate([FromBody] User model)
        {

            var role = "User";
            if (model.UserName == "admin")
            {
                role = "Admin";
            } //test

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Convert.FromBase64String(ConfigurationManager.AppSettings["jwtSecret"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, model.UserName),
                    new Claim(ClaimTypes.Role, role),
                    new Claim(ClaimTypes.Email, "rayhankabir.wp@gmail.com"),
                }),
                Expires = DateTime.UtcNow.AddMinutes(10),
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





    