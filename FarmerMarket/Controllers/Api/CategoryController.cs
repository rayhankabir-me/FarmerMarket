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
    public class CategoryController : ApiController
    {
        FarmerMarketContext _dbContext;
        public CategoryController()
        {
            _dbContext = new FarmerMarketContext();
        }

        [Route("api/categories")]
        public IEnumerable<Category> GetCategories()
        {
            return _dbContext.Categories.ToList();
        }

        [Route("api/categories/{id}")]
        public IHttpActionResult GetCategory(int id)
        {
            var category = _dbContext.Categories.FirstOrDefault(x => x.CategoryId == id);
            if (category == null)
                return NotFound();

            return Ok(category);
        }

        [Authorize(Roles = "Admin")]
        [Route("api/add-category")]
        [HttpPost]
        public IHttpActionResult PostCategory(Category category)
        {
            string lowerCategoryName = category.Name.Trim().ToLower();

            var existingCategory = _dbContext.Categories.FirstOrDefault(c => c.Name.Trim().ToLower() == lowerCategoryName);

            if (existingCategory != null)
            {
                return BadRequest("Category name already exists!");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _dbContext.Categories.Add(category);
            _dbContext.SaveChanges();

            return Ok(category);
        }


        [Authorize(Roles = "Admin")]
        [Route("api/edit-category/{id}")]
        [HttpPut]
        public IHttpActionResult EditCategory(int id, Category category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var data = _dbContext.Categories.FirstOrDefault(x => x.CategoryId == id);
            if (data == null)
            {
                return NotFound();
            }

            data.Name = category.Name;
            _dbContext.SaveChanges();

            return Ok();
        }
    }
}
