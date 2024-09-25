using FarmerMarket.Context;
using FarmerMarket.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FarmerMarket.Controllers.Api
{
    public class UserController : ApiController
    {

        FarmerMarketContext _dbContext;
        public UserController()
        {
            _dbContext = new FarmerMarketContext();
        }

        [Route("api/users")]
        public IEnumerable<object> GetUsers()
        {
            return _dbContext.Users.Select(u => new { u.UserId, u.UserName, u.Email, u.Role }).ToList();
        }

        [Route("api/users/{id}")]
        public IHttpActionResult GetUser(int id)
        {
            var user = _dbContext.Users.Where(x => x.UserId == id).Select(u => new { u.UserId, u.UserName, u.Email, u.Role }).FirstOrDefault();

            if (user == null)
                return NotFound();

            return Ok(user);
        }


        [Route("api/users/register")]
        [HttpPost]
        public IHttpActionResult PostUser(User user)
        {
            string lowercaseUserName = user.UserName.Trim().ToLower();
            string lowercaseEmail = user.Email.Trim().ToLower();

            var existingUserName = _dbContext.Users.FirstOrDefault(u => u.UserName.Trim().ToLower() == lowercaseUserName);
            var existingEmail = _dbContext.Users.FirstOrDefault(u => u.Email.Trim().ToLower() == lowercaseEmail);

            if (existingUserName != null)
            {
                return BadRequest("Username already exists!");
            }

            if (existingEmail != null)
            {
                return BadRequest("Email already exists! Try a different one.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

       
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();

            return Ok(user);
        }

        [Route("api/edit-user/{id}")]
        [HttpPut]
        public IHttpActionResult EditUser(int id, User user)
        {

            string lowercaseUserName = user.UserName.Trim().ToLower();
            string lowercaseEmail = user.Email.Trim().ToLower();

            var existingUserName = _dbContext.Users.FirstOrDefault(u => u.UserName.Trim().ToLower() == lowercaseUserName);
            var existingEmail = _dbContext.Users.FirstOrDefault(u => u.Email.Trim().ToLower() == lowercaseEmail);

            if (existingUserName != null)
            {
                return BadRequest("Username already exists!");
            }

            if (existingEmail != null)
            {
                return BadRequest("Email already exists! Try a different one.");
            }


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var data = _dbContext.Users.FirstOrDefault(x => x.UserId == id);
            if (data == null)
            {
                return NotFound();
            }

            data.UserName = user.UserName;
            data.Email = user.Email;
            data.Password = user.Password;
            data.Role = user.Role;

            _dbContext.SaveChanges();
            return Ok();
        }

        [Route("api/users/delete/{id}")]
        [HttpDelete]
        public IHttpActionResult DeleteUser(int id)
        {
            var user = _dbContext.Users.FirstOrDefault(x => x.UserId == id);
            if (user == null)
                return NotFound();

            _dbContext.Users.Remove(user);
            _dbContext.SaveChanges();

            return Ok();
        }



    }
}
