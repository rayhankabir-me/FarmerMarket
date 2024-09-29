using FarmerMarket.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.Entity;
using FarmerMarket.Models;
using System.Security.Claims;

namespace FarmerMarket.Controllers.Api
{
    public class PostController : ApiController
    {
        FarmerMarketContext _dbContext;
        public PostController()
        {
            _dbContext = new FarmerMarketContext();
        }

        [Route("api/posts")]
        public IEnumerable<object> GetPosts()
        {
            return _dbContext.Posts
                .Include(p => p.User)
                .Select(p => new
                {
                    p.PostId,
                    p.Title,
                    p.Description,
                    p.PostDate,
                    p.ImageUrl,
                    User = new
                    {
                        p.User.UserId,
                        p.User.UserName,
                        p.User.Email
                    }
                })
                .ToList();
        }



        [Route("api/post/{id}")]
        public IHttpActionResult Getpost(int id)
        {
            var post = _dbContext.Posts
                .Include(p => p.User)
                .Where(p => p.PostId == id)
                .Select(p => new
                {
                    p.Title,
                    p.Description,
                    p.PostDate,
                    p.ImageUrl,
                    User = new
                    {
                        p.User.UserId,
                        p.User.UserName,
                        p.User.Email
                    }
                }).FirstOrDefault();
            if (post == null)
                return NotFound();

            return Ok(post);
        }

        [Authorize(Roles = "Admin")]
        [Route("api/create-post")]
        [HttpPost]
        public IHttpActionResult CreatePost(Post post)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //getting the UserId from the token
            var identity = User.Identity as ClaimsIdentity;
            var userIdClaim = identity?.FindFirst("UserId");

            if (userIdClaim == null)
            {
                return Unauthorized();
            }

            //setting the UserId in the Product
            int userId = int.Parse(userIdClaim.Value);
            post.UserId = userId;

            _dbContext.Posts.Add(post);
            _dbContext.SaveChanges();

            return Ok(post);
        }

        [Authorize(Roles = "Admin")]
        [Route("api/edit-post/{id}")]
        [HttpPut]
        public IHttpActionResult EditProduct(int id, Post post)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var data = _dbContext.Posts.FirstOrDefault(x => x.PostId == id);
            if (data == null)
            {
                return NotFound();
            }

            data.Title = post.Title;
            data.Description = post.Description;
            _dbContext.SaveChanges();

            return Ok(data);
        }

        [Authorize(Roles = "Admin")]
        [Route("api/delete-post/{id}")]
        [HttpDelete]
        public IHttpActionResult DeleteGame(int id)
        {
            var data = _dbContext.Posts.FirstOrDefault(x => x.PostId == id);
            if (data == null)
                return NotFound();

            _dbContext.Posts.Remove(data);
            _dbContext.SaveChanges();

            return Ok("Post has been deleted!!");
        }


        [Route("api/posts/search")]
        [HttpGet]
        public IHttpActionResult SearchPosts(string term = "")
        {
            var query = _dbContext.Posts
                                  .Include(p => p.User)
                                  .AsQueryable();

            if (!string.IsNullOrEmpty(term))
            {
                query = query.Where(p => p.Title.Contains(term) || p.Description.Contains(term));
            }

            var posts = query.Select(p => new
            {
                p.PostId,
                p.Title,
                p.Description,
                p.PostDate,
                p.ImageUrl,
                User = new
                {
                    p.User.UserId,
                    p.User.UserName,
                    p.User.Email
                }
            }).ToList();

            if (!posts.Any())
            {
                return NotFound();
            }
            return Ok(posts);
        }
    }
}
