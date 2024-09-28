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
    public class CartController : ApiController
    {

        public FarmerMarketContext _dbContext;

        public CartController()
        {
            _dbContext = new FarmerMarketContext();
        }

        [Authorize(Roles = "Customer")]
        [Route("api/cart/getcart")]
        [HttpGet]
        public IHttpActionResult GetCart()
        {
            var identity = User.Identity as ClaimsIdentity;
            var userIdClaim = identity?.FindFirst("UserId");

            if (userIdClaim == null)
            {
                return Unauthorized();
            }

            int userId = int.Parse(userIdClaim.Value);

            var cart = _dbContext.Carts
                .Include(c => c.CartItems.Select(ci => ci.Product))
                .FirstOrDefault(c => c.UserId == userId);


            if (cart == null)
            {
                return NotFound();
            }

            var cartResponse = new
            {
                CartItems = cart.CartItems.Select(ci => new
                {
                    ItemId = ci.ItemId,
                    ProductName = ci.Product.Name,
                    Quantity = ci.Quantity,
                    TotalPrice = ci.TotalPrice
                }).ToList()
            };

            return Ok(cartResponse);
        }


        [Authorize(Roles = "Customer")]
        [Route("api/cart/addcart")]
        [HttpPost]
        public IHttpActionResult AddProductToCart(CartItem CartIteams)
        {
            // Get the user ID from the token
            var identity = User.Identity as ClaimsIdentity;
            var userIdClaim = identity?.FindFirst("UserId");

            if (userIdClaim == null)
            {
                return Unauthorized();
            }

            int userId = int.Parse(userIdClaim.Value);

            var existingCart = _dbContext.Carts.FirstOrDefault(c => c.UserId == userId);

            if (existingCart == null)
            {
                existingCart = new Cart
                {
                    UserId = userId
                };
                _dbContext.Carts.Add(existingCart);
                _dbContext.SaveChanges();
            }

            var product = _dbContext.Products.FirstOrDefault(p => p.ProductId == CartIteams.ProductId);
            if (product == null)
            {
                return NotFound();
            }

            if (product.Stock < 2)
            {
                return BadRequest("Product cannot be added to the cart as stock is less than 2.");
            }

            int quantity = 1;

            var cartItem = _dbContext.CartItems.FirstOrDefault(ci =>
                ci.CartId == existingCart.CartId && ci.ProductId == product.ProductId);

            if (cartItem == null)
            {
                cartItem = new CartItem
                {
                    ProductId = product.ProductId,
                    Quantity = quantity,
                    CartId = existingCart.CartId,
                    TotalPrice = (decimal)(product.Price * quantity)
                };
                _dbContext.CartItems.Add(cartItem);
            }
            else
            {
                cartItem.Quantity += quantity;
                cartItem.TotalPrice = (decimal)(product.Price * cartItem.Quantity);
            }

            _dbContext.SaveChanges();

            return Ok("Product added to cart!");
        }


        [Authorize(Roles = "Customer")]
        [Route("api/cart/edit-item")]
        [HttpPut]
        public IHttpActionResult EditCartItem(CartItem cartItem)
        {
            // Extract UserId from the token
            var identity = User.Identity as ClaimsIdentity;
            var userIdClaim = identity?.FindFirst("UserId");

            if (userIdClaim == null)
            {
                return Unauthorized(); // If there's no UserId in the token
            }

            int userId = int.Parse(userIdClaim.Value);

            // Find the user's cart
            var cart = _dbContext.Carts.FirstOrDefault(c => c.UserId == userId);

            if (cart == null)
            {
                return NotFound(); // No cart found for the user
            }

            // Find the cart item to update
            var existingCartItem = _dbContext.CartItems.FirstOrDefault(ci => ci.CartId == cart.CartId && ci.ItemId == cartItem.ItemId);

            if (existingCartItem == null)
            {
                return NotFound(); // No cart item found to update
            }

            // Validate new quantity
            if (cartItem.Quantity <= 0)
            {
                return BadRequest("Quantity must be greater than zero."); // Check for valid quantity
            }

            // Update the quantity and calculate the new total price
            existingCartItem.Quantity = cartItem.Quantity;

            var product = _dbContext.Products.FirstOrDefault(p => p.ProductId == existingCartItem.ProductId);
            if (product != null)
            {
                existingCartItem.TotalPrice = (decimal)(product.Price * existingCartItem.Quantity); // Update total price
            }

            _dbContext.SaveChanges(); // Save changes to the database

            return Ok("Cart item updated successfully!");
        }




        [Authorize(Roles = "Customer")]
        [Route("api/cart/remove-product/{itemId}")]
        [HttpDelete]
        public IHttpActionResult RemoveCartItem(int itemId)
        {
            var cartItem = _dbContext.CartItems.FirstOrDefault(ci => ci.ItemId == itemId);
            if (cartItem == null)
            {
                return NotFound();
            }

            _dbContext.CartItems.Remove(cartItem);
            _dbContext.SaveChanges();

            return Ok("Item removed from cart!");
        }


    }
}
