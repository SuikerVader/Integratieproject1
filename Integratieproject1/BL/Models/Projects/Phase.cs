using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Integratieproject1.BL.Models.Ideations;
using Integratieproject1.BL.Models.Surveys;

namespace Integratieproject1.BL.Models.Projects
{
    public class Phase
    {
      [Key]
        public int PhaseId { get; set; }
    [Required]
        public int PhaseNr { get; set; }
        public string PhaseName { get; set; }
        public string Description { get; set; }
    [Required]
        public DateTime StartDate { get; set; }
    [Required]
        public DateTime EndDate { get; set; }

    [Required]
    public Project Project { get; set; }

    public IList<Ideation> Ideations { get; set; }
    public IList<Survey> Surveys { get; set; }
    
    /*[Required]
    [ForeignKey("Project")]
    public int ProjectFK { get; set; }*/
    
  }
}
