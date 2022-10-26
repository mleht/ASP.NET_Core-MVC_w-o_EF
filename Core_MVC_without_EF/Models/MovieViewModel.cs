using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Core_MVC_without_EF.Models
{
    public class MovieViewModel
    {
        [Key]
        public int MovieID { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Director { get; set; }
        [Range(1,10,ErrorMessage = "Should be between 1 - 10")]
        public int Rating { get; set; }
    }
}
