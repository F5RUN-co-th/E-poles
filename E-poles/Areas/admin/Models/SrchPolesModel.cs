using E_poles.Models.Datables;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_poles.Areas.admin.Models
{
    public class SrchPolesModel : DtParameters
    {
        public string KeySearch { get; set; }
    }
}
