using FarmerMarket.Context;
using FarmerMarket.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Http;
using System.Web.Http;

namespace FarmerMarket.Controllers.Api
{
    public class ContactController : ApiController
    {
        FarmerMarketContext _dbContext;
        public ContactController()
        {
            _dbContext = new FarmerMarketContext();
        }

        [Authorize(Roles = "Admin")]
        [Route("api/contacts")]
        public IEnumerable<object> GetContacts()
        {
            return _dbContext.Contacts
                .Select(c => new
                {
                    c.ContactId,
                    c.FirstName,
                    c.LastName,
                    c.Email,
                    c.Subject,
                    c.Message,
                })
                .ToList();
            //return _dbContext.Contacts.ToList();
        }

        [Authorize(Roles = "Admin")]
        [Route("api/contact/{id}")]
        public IHttpActionResult GetContact(int id)
        {
            var contact = _dbContext.Contacts.FirstOrDefault(x => x.ContactId == id);

            if (contact == null)
                return NotFound();

            return Ok(contact);
        }

        [Route("api/contact/request")]
        [HttpPost]
        public IHttpActionResult PostRequest(Contact contact)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Check if the subject already exists
            var existingContact = _dbContext.Contacts
                .FirstOrDefault(c => c.Subject.Equals(contact.Subject, StringComparison.OrdinalIgnoreCase));

            if (existingContact != null)
            {
                return BadRequest("Please choose a different subject.");
            }

            _dbContext.Contacts.Add(contact);
            _dbContext.SaveChanges();

            return Ok(contact);
        }

        [Authorize(Roles = "Admin")]
        [Route("api/delete-contact/{id}")]
        [HttpDelete]
        public IHttpActionResult DeleteContact(int id)
        {
            var contact = _dbContext.Contacts.FirstOrDefault(x => x.ContactId == id);
            if (contact == null)
                return NotFound();

            _dbContext.Contacts.Remove(contact);
            _dbContext.SaveChanges();

            return Ok("This contact request has been deleted!");
        }
    }
}
