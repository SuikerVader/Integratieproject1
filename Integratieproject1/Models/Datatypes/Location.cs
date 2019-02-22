using Integratieproject1.Models.Datatypes;
using Integratieproject1.Models.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Integratieproject1.Models
{
    public class Location
    {
        public String locationName { get; set; }
        public Address address { get; set; }
        public Position position { get; set; }
    }
}
