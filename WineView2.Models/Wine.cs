using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;


namespace WineView2.Models
{
    public class Wine
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Price for 1-5")]
        [Range(1, 10000)]
        public double Price { get; set; }

        [Required]
        [Display(Name = "Price for 5-10")]
        [Range(1, 10000)]
        public double Price5 { get; set; }

        [Required]
        [Display(Name = "Price for 10+")]
        [Range(1, 10000)]
        public double Price10 { get; set; }

        [Required]
        [Display(Name = "Color")]
        public int ColorId { get; set; }

        [ForeignKey("ColorId")]
        [ValidateNever]
        public Color Color { get; set; }

        [ValidateNever]
        public string ImageUrl { get; set; }
    }
}
