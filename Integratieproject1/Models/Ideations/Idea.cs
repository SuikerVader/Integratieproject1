using Integratieproject1.Models.Datatypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Integratieproject1.Models.Ideations
{
    public class Idea
    {
        public Position position { get; set; }
        public String video { get; set; }
        public String image { get; set; }
        public String theme { get; set; }
        public String text { get; set; }
    }
}
