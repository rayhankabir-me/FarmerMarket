using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FarmerMarket.Models
{
    public class Cart
    {
        [Key]
        public int CartId { get; set; }
        [Required]
        public int Quantity { get; set; }
        public double TotalPrice { get; set; }
        public int ProductId { get; set; }
        public int UserId { get; set; }

        public User User { get; set; }
        public Product Product { get; set; }

    }
}