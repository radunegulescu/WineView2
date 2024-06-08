using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WineView2.Models
{
    public class Grape
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Grape")]
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [ValidateNever]
        [JsonIgnore]
        public List<Wine> Wines { get; set; }
    }
}
