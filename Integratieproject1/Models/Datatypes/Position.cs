using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Integratieproject1.Models.Datatypes
{
    public class Position
    {   
    [Key]
        public int PostionId { get; set; }
        public double Longitude { get; set; }
        public double Altitude { get; set; }
    }
}
