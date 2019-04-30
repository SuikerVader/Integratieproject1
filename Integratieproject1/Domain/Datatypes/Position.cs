using System.ComponentModel.DataAnnotations;

namespace Integratieproject1.Domain.Datatypes
{
    public class Position
    {   
    [Key]
        public int PositionId { get; set; }
        public string Lng { get; set; }
        public string Lat { get; set; }
    }
}
