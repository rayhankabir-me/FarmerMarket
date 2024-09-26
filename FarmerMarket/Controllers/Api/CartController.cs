using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlTypes;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using FarmerMarket.Context;
using FarmerMarket.Models;

namespace FarmerMarket.Controllers.Api
{
    public class CartController : ApiController
    {
        FarmerMarketContext _dbContext;

        public CartController()
        {
            _dbContext = new FarmerMarketContext();
        }

        [Route("api/cart")]
        public IEnumerable<object> GetCarts()
        {
            return _dbContext.Carts.Include(x => x.Product)
                .Include(x => x.User)
                .Select(x => new
                {
                    x.Quantity,
                    x.TotalPrice,
                    User = new
                    {
                        x.User.UserName,
                    },
                    Product = new
                    {
                        x.Product.Name,
                        x.Product.Price
                    }


                }).ToList();
        }



        [Route("api/cart/{id}")]
        public IHttpActionResult GetCart(int id)
        {
            var cart = _dbContext.Carts.FirstOrDefault(x => x.UserId == id);
            if (User == null)
            {
                return NotFound();
            }

            var carts = _dbContext.Carts.Include(x => x.Product)
                .Include(x => x.User)
                .Where(x => x.UserId == id)
                .Select(x => new
                {
                    x.Quantity,
                    x.TotalPrice,
                    User = new
                    {
                        x.User.UserName,
                    },
                    Product = new
                    {
                        x.Product.Name,
                        x.Product.Price
                    }
                }).ToList();
            return Ok(carts);
        }



        [Route("api/cart/add-to-cart")]
        [HttpPost]
        public IHttpActionResult AddToCart(Cart cart)
        {
            //var username = HttpContext.Current.Session["Username"] as string;

            //if (string.IsNullOrEmpty(username))
            //{
            //    return BadRequest("User is not logged in.");
            //}

            //var user = _dbContext.Users.FirstOrDefault(u => u.UserName == username);

            //if (user == null)
            //{
            //    return BadRequest("Invalid user.");
            //}

            //cart.UserId = user.UserId;

            var product = _dbContext.Products.FirstOrDefault(p => p.ProductId == cart.ProductId);

            if (product == null)
            {
                return BadRequest("Invalid product.");
            }

            if (cart.Quantity <= 0)
            {
                cart.Quantity = 1;
            }

            cart.TotalPrice = product.Price * cart.Quantity;

            _dbContext.Carts.Add(cart);
            _dbContext.SaveChanges();

            return Ok("Product added to cart successfully.");
        }


        [Route("api/cart/edit-cart/{id}")]
        [HttpPut]
        public IHttpActionResult EditCart(int id, Cart cart)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var update = _dbContext.Carts.Include(x => x.Product).FirstOrDefault(x => x.CartId == id);

            if (update == null)
            {
                return NotFound();
            }

            update.Quantity = cart.Quantity;
            update.TotalPrice = update.Product.Price * update.Quantity;
            _dbContext.SaveChanges();

            return Ok("Cart updated successfully.");
        }


    }
}
