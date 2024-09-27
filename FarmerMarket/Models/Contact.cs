using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FarmerMarket.Models
{
    public class Contact
    {
        [Key]
        public int ContactId { get; set; }

        [Required(ErrorMessage = "First name field is empty.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name field is empty.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email address is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Write the subject of the request")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "Write the body of the request")]
        public string Message { get; set; }
    }
}