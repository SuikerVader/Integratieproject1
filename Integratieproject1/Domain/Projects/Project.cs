using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Integratieproject1.Domain.Datatypes;
using Integratieproject1.Domain.Users;

namespace Integratieproject1.Domain.Projects
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
        
        public ICollection<Phase> Phases { get; set; }
        public ICollection<AdminProject> AdminProjects { get; set; }
    }

    
}
