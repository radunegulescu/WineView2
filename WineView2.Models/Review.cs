using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WineView2.Models
{
    public class Review
    {
        public int Id { get; set; }

        public string Text { get; set; }

        [Range(1, 10)]
        [Required]
        public int Sweetness { get; set; }

        [Range(1, 10)]
        [Required]
        public int Acidity { get; set; }

        [Range(1, 10)]
        [Required]
        public int Tannin { get; set; }

        [Required]
        [Display(Name = "Body")]
        public int BodyId { get; set; }

        [ValidateNever]
        public Body Body { get; set; }

        [ValidateNever]
        [Display(Name = "Sentiment Score")]
        public double SentimentScore { get; set; }

        [Required]
        [Display(Name = "User")]
        public string ApplicationUserId { get; set; }

        [ValidateNever]
        public ApplicationUser ApplicationUser { get; set; }

        [Required]
        [Display(Name = "Wine")]
        public int WineId { get; set; }

        [ValidateNever]
        public Wine Wine { get; set; }
    }
}
