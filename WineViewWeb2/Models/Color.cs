﻿using System.ComponentModel.DataAnnotations;

namespace WineViewWeb2.Models
{
    public class Color
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Color")]
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
    }
}
