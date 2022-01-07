using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace E_poles.Areas.admin.Models
{
    public class DashboardViewModel
    {
        public int TotalPole { get; set; }
        public int TotalAdmin { get; set; }
        public int TotalUser { get; set; }
    }
}
