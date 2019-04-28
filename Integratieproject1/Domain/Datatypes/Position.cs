using System.ComponentModel.DataAnnotations;

namespace Integratieproject1.Domain.Datatypes
{
    public class Position
    {   
    [Key]
        public int PostionId { get; set; }
        public string Lng { get; set; }
        public string Lat { get; set; }
    }
}
