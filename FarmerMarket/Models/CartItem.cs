using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FarmerMarket.Models
{
    public class CartItem
    {
        [Key]
        public int ItemId { get; set; }
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }

        public int CartId { get; set; }
        public virtual Cart Cart { get; set; }
    }
}