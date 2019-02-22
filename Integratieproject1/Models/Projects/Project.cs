using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Integratieproject1.Models.Projects
{
    public class Project
    {
        public String projectName { get; set; }
        public String logo { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public String objective { get; set; }
        public String description { get; set; }
        public Location location { get; set; }
        public String status { get; set; }

    }
}
