using E_poles.Models.Datables;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_poles.Areas.admin.Models
{
    public class SrchUserModel : DtParameters
    {
        public string KeySearch { get; set; }
        public string UserId { get; set; }
    }
}
