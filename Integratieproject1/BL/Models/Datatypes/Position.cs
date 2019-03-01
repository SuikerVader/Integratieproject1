using System.ComponentModel.DataAnnotations;

namespace Integratieproject1.BL.Models.Datatypes
{
    public class Position
    {   
    [Key]
        public int PostionId { get; set; }
        public double Longitude { get; set; }
        public double Altitude { get; set; }
    }
}
