﻿using FarmerMarket.Context;
using FarmerMarket.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.Entity;

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

        [Route("api/products/{id}")]
        public IHttpActionResult GetProduct(int id)
        {

            var productExists = _dbContext.Products.Any(p => p.ProductId == id);
            if (!productExists)
            {
                return Content(HttpStatusCode.NotFound, $"Product with ID {id} not found.");
            }


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



        [Route("api/add-product")]
        [HttpPost]
        public IHttpActionResult PostProducts(Product product)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _dbContext.Products.Add(product);
            _dbContext.SaveChanges();

            return Ok(product);
        }

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



    }
}
