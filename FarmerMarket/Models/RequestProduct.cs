using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FarmerMarket.Models
{
    public class RequestProduct
    {
        [Key]
        public int ReqId { get; set; }
        [Required]
        [MaxLength(20)]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MaxLength(20)]
        public string ProductName { get; set; }

        [Required]
        [MaxLength(11)]
        [MinLength(11)]
        public string phone { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime Time { get; set; } = DateTime.Now;

        [MaxLength(25)]
        public string Massage { get; set; }
    }
}