using E_poles.Models.Datables;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        public string SelectedStatus { get; set; }

        public string SelectedArea { get; set; }
        public string SelectedStreet { get; set; }
        public List<SelectListItem> AreaList { get; set; }
        public List<SelectListItem> StreetList { get; set; }
    }
}
