using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace E_poles.Areas.admin.Models
{
    public class PoleViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public decimal Latitude { get; set; }
        [Required]
        public decimal Longitude { get; set; }
        [Required]
        public string Area { get; set; }
        [Required]
        public string Street { get; set; }
        public string Note { get; set; }
        public string Description { get; set; }
        public bool Status { get; set; }
    }
}
