using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_poles.Models.Pole
{
    public class PoleListModel
    {
        public int Id { get; set; }
        public string FullName
        {
            get
            {
                return Street + " " + Area + " " + Name;
            }
        }

        public string Name { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string Area { get; set; }
        public string Street { get; set; }
        public string Note { get; set; }
        public string Description { get; set; }
        public bool Status { get; set; }
    }
}
