using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Integratieproject1.Models.Datatypes;

namespace Integratieproject1.Models.Projects
{
    public class Project
    {
        [Key]
        public int ProjectId { get; set; }
    [Required]
        public string ProjectName { get; set; }
        public string Logo { get; set; }
    [Required]
        public DateTime StartDate { get; set; }
    [Required]
        public DateTime EndDate { get; set; }
        public string Objective { get; set; }
        public string Description { get; set; }
    [Required]
        public Location Location { get; set; }
        public string Status { get; set; }
     
        [Required] 
        public Platform Platform { get; set; }
        
        public IList<Phase> Phases { get; set; }
    }
}
