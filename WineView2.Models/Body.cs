using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WineView2.Models
{
    public class Body
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Body")]
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
    }
}
