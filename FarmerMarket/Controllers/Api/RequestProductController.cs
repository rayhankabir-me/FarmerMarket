using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using FarmerMarket.Context;
using FarmerMarket.Models;

namespace FarmerMarket.Controllers.Api
{
    public class RequestProductController : ApiController
    {
        FarmerMarketContext _dbContext;

        public RequestProductController()
        {
            _dbContext = new FarmerMarketContext();
        }


        [Authorize(Roles = "Admin")]
        [Route("api/RequestProduct")]
        public IHttpActionResult GetAllRequest()
        {
            var identity = User.Identity as ClaimsIdentity;
            var userIdClaim = identity?.FindFirst("UserId");

            if (userIdClaim == null)
            {
                return Unauthorized();
            }

            int userId = int.Parse(userIdClaim.Value);

            var response = _dbContext.RequestProducts.ToList();

            if (response == null || response.Count == 0)
            {
                return NotFound();
            }

            return Ok(response);
        }

        [Route("api/requestproduct/addrequest")]
        [HttpPost]
        public IHttpActionResult AddRequest(RequestProduct requestProduct)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _dbContext.RequestProducts.Add(requestProduct);
            _dbContext.SaveChanges();
            return Ok("Request Submitted");

        }
    }
}
