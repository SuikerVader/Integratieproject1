using Integratieproject1.Models.Datatypes;
using Integratieproject1.Models.Projects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Integratieproject1.Models
{
    public class Location
    {
     [Key]
        public int LocationId { get; set; }
        public String LocationName { get; set; }
        public Address Address { get; set; }
        public Position Position { get; set; }
    }
}
