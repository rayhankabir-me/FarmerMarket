using FarmerMarket.Context;
using FarmerMarket.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.Entity;
using System.Security.Claims;

namespace FarmerMarket.Controllers.Api
{
    public class ProductController : ApiController
    {
        FarmerMarketContext _dbContext;
        public ProductController()
        {
            _dbContext = new FarmerMarketContext();
        }

        [Route("api/products")]
        public IEnumerable<object> GetProducts()
        {
            return _dbContext.Products
                             .Include(p => p.User)        
                             .Include(p => p.Category)    
                             .Select(p => new
                             {
                                 p.ProductId,
                                 p.Name,
                                 p.Price,
                                 p.Description,
                                 p.Stock,
                                 p.ImageUrl,
                                 User = new
                                 {
                                     p.User.UserId,
                                     p.User.UserName,
                                     p.User.Email
                                 },
                                 Category = new
                                 {
                                     p.Category.CategoryId,
                                     p.Category.Name
                                 }
                             })
                             .ToList();
        }

        //for home
        [Route("api/products/home")]
        public IEnumerable<object> GetProductsHome()
        {
            return _dbContext.Products
                             .Include(p => p.User)
                             .Include(p => p.Category)
                             .OrderByDescending(p => p.ProductId)
                             .Take(4)
                             .Select(p => new
                             {
                                 p.ProductId,
                                 p.Name,
                                 p.Price,
                                 p.Description,
                                 p.Stock,
                                 p.ImageUrl,
                                 User = new
                                 {
                                     p.User.UserId,
                                     p.User.UserName,
                                     p.User.Email
                                 },
                                 Category = new
                                 {
                                     p.Category.CategoryId,
                                     p.Category.Name
                                 }
                             })
                             .ToList();
        }




        [Route("api/products/{id}")]
        public IHttpActionResult GetProductById(int id)
        {
            var product = _dbContext.Products
                                    .Include(p => p.User)        
                                    .Include(p => p.Category)    
                                    .Where(p => p.ProductId == id) 
                                    .Select(p => new
                                    {
                                        p.ProductId,
                                        p.Name,
                                        p.Price,
                                        p.Description,
                                        p.Stock,
                                        p.ImageUrl,
                                        User = new
                                        {
                                            p.User.UserId,
                                            p.User.UserName,
                                            p.User.Email
                                        },
                                        Category = new
                                        {
                                            p.Category.CategoryId,
                                            p.Category.Name
                                        }
                                    })
                                    .FirstOrDefault();  

            if (product == null)
                return NotFound();  

            return Ok(product);  
        }



        [Authorize(Roles = "Admin")]
        [Route("api/add-product")]
        [HttpPost]
        public IHttpActionResult PostProducts(Product product)
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
            product.UserId = userId;

            _dbContext.Products.Add(product);
            _dbContext.SaveChanges();

            return Ok(product);
        }

        [Authorize(Roles = "Admin")]
        [Route("api/edit-product/{id}")]
        [HttpPut]
        public IHttpActionResult EditProduct(int id, Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var data = _dbContext.Products.FirstOrDefault(x => x.ProductId == id);
            if (data == null)
            {
                return NotFound();
            }

            data.Name = product.Name;
            data.Price = product.Price;
            data.Description = product.Description;
            data.Stock = product.Stock;
            data.ImageUrl = product.ImageUrl;
            data.CategoryId = product.CategoryId;
            _dbContext.SaveChanges();

            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [Route("api/products/delete/{id}")]
        [HttpDelete]
        public IHttpActionResult DeleteProduct(int id)
        {
            var product = _dbContext.Products.FirstOrDefault(x => x.ProductId == id);
            if (product == null)
                return NotFound();

            _dbContext.Products.Remove(product);
            _dbContext.SaveChanges();

            return Ok();
        }


        [Route("api/products/search")]
        [HttpGet]
        public IHttpActionResult SearchProducts(string term = "", int? CategoryId = null)
        {
            var query = _dbContext.Products
                                  .Include(p => p.User)          
                                  .Include(p => p.Category)      
                                  .AsQueryable();               

            if (!string.IsNullOrEmpty(term))
            {
                query = query.Where(p => p.Name.Contains(term) || p.Description.Contains(term));
            }

            if (CategoryId.HasValue)
            {
                query = query.Where(p => p.CategoryId == CategoryId.Value);
            }

            var products = query.Select(p => new
            {
                p.ProductId,
                p.Name,
                p.Price,
                p.Description,
                p.Stock,
                p.ImageUrl,
                User = new
                {
                    p.User.UserId,
                    p.User.UserName,
                    p.User.Email
                },
                Category = new
                {
                    p.Category.CategoryId,
                    p.Category.Name
                }
            }).ToList();

            if (!products.Any())
            {
                return NotFound();
            }
            return Ok(products); 
        }





    }
}
