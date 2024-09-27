using FarmerMarket.Context;
using System;
using FarmerMarket.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.Entity;
using System.Security.Claims;

namespace FarmerMarket.Controllers.Api
{
    public class OrderController : ApiController
    {
        FarmerMarketContext _dbContext;
        public OrderController()
        {
            _dbContext = new FarmerMarketContext();
        }

        [Authorize(Roles = "Admin")]
        [Route("api/orders")]
        public IEnumerable<object> GetOrders()
        {
            return _dbContext.Orders
                             .Include(o => o.User)
                             .Include(o => o.Product)
                             .Select(o => new
                             {
                                 o.OrderId,
                                 o.OrderDate,
                                 o.TotalPrice,
                                 o.Quantity,
                                 o.PaymentMethod,
                                 o.PaymentStatus,
                                 o.DeliveryAddress,
                                 o.OrderStatus,
                                 User = new
                                 {
                                     o.User.UserId,
                                     o.User.UserName,
                                     o.User.Email
                                 },
                                 Product = new
                                 {
                                     o.Product.ProductId,
                                     o.Product.Name,
                                     o.Product.Description,
                                     o.Product.Price
                                 }
                             })
                             .ToList();
        }

        [Authorize(Roles = "Customer")]
        [Route("api/orders/{id}")]
        public IHttpActionResult GetOrderById(int id)
        {
            var order = _dbContext.Orders
                             .Include(o => o.User)
                             .Include(o => o.Product)
                             .Where(p => p.OrderId == id)
                             .Select(o => new
                             {
                                 o.OrderId,
                                 o.OrderDate,
                                 o.TotalPrice,
                                 o.Quantity,
                                 o.PaymentMethod,
                                 o.PaymentStatus,
                                 o.DeliveryAddress,
                                 o.OrderStatus,
                                 User = new
                                 {
                                     o.User.UserId,
                                     o.User.UserName,
                                     o.User.Email
                                 },
                                 Product = new
                                 {
                                     o.Product.ProductId,
                                     o.Product.Name,
                                     o.Product.Description,
                                     o.Product.Price
                                 }
                             })
                             .ToList();

            if (order == null)
            {
                return NotFound();
            }
            return Ok(order);
        }

        [Authorize(Roles = "Customer")]
        [Route("api/place-order")]
        [HttpPost]
        public IHttpActionResult PostOrders(Order order)
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
            order.UserId = userId;

            var existingOrder = _dbContext.Orders
                .FirstOrDefault(o => o.UserId == order.UserId && o.ProductId == order.ProductId);

            if (existingOrder != null)
            {
                return BadRequest("You have already placed an order for this product.");
            }

            _dbContext.Orders.Add(order);
            _dbContext.SaveChanges();

            return Ok(order);
        }

        [Authorize(Roles = "Customer")]
        [Route("api/edit-order/{id}")]
        [HttpPut]
        public IHttpActionResult EditOrder(int id, Order order)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var data = _dbContext.Orders.FirstOrDefault(x => x.OrderId == id);
            if (data == null)
            {
                return NotFound();
            }

            data.OrderDate = order.OrderDate;
            data.TotalPrice = order.TotalPrice;
            data.Quantity = order.Quantity;
            data.PaymentMethod = order.PaymentMethod;
            data.PaymentStatus = order.PaymentStatus;
            data.DeliveryAddress = order.DeliveryAddress;
            data.OrderStatus = order.OrderStatus;
            data.ProductId = order.ProductId;
            _dbContext.SaveChanges();

            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [Route("api/delete-order/{id}")]
        [HttpDelete]
        public IHttpActionResult DeleteGame(int id)
        {
            var data = _dbContext.Orders.FirstOrDefault(x => x.OrderId == id);
            if (data == null)
                return NotFound();

            _dbContext.Orders.Remove(data);
            _dbContext.SaveChanges();

            return Ok();
        }

        public IHttpActionResult SearchOrder()
        { return Ok(); }
    }
}