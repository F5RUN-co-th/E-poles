using System;
using System.ComponentModel.DataAnnotations.Schema;
namespace E_poles.Dal
{
    public class Poles
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [Column(TypeName = "decimal(18, 15)")]
        public decimal Latitude { get; set; }
        [Column(TypeName = "decimal(18, 15)")]
        public decimal Longitude { get; set; }
        public string Area { get; set; }
        public string Street { get; set; }
        public string Note { get; set; }
        public string Description { get; set; }
        public bool Status { get; set; }

        public User Users { get; set; }

        public int GroupsId { get; set; }
        public Groups Groups { get; set; }

        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
    }
}
