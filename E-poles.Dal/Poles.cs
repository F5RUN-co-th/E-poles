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
    }
}
