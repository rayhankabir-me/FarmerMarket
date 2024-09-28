using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FarmerMarket.Models
{
    public class Post
    {
        [Key]
        public int PostId { get; set; }

        [Required]
        [MinLength(20, ErrorMessage = "Write a bit more, c'mon!")]
        public string Title { get; set; }

        [Required]
        [MinLength(50, ErrorMessage = "Go on, write more!")]
        public string Description { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "0:yyyy-MM-dd", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> PostDate { get; set; }

        public string ImageUrl { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }

    }
}