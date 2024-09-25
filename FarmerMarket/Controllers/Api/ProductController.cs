using FarmerMarket.Context;
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



    }
}
