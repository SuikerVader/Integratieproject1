using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Integratieproject1.Models.Projects
{
    public class Project
    {
        [Key]
        public int ProjectId { get; set; }
    [Required]
        public String ProjectName { get; set; }
        public String Logo { get; set; }
    [Required]
        public DateTime StartDate { get; set; }
    [Required]
        public DateTime EndDate { get; set; }
        public String Objective { get; set; }
        public String Description { get; set; }
    [Required]
        public Location Location { get; set; }
        public String Status { get; set; }

    }
}
