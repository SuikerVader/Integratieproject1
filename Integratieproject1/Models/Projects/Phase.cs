using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Integratieproject1.Models.Projects
{
    public class Phase
    {
      [Key]
        public int PhaseId { get; set; }
    [Required]
        public int PhaseNr { get; set; }
        public String PhaseName { get; set; }
        public String Description { get; set; }
    [Required]
        public DateTime StartDate { get; set; }
    [Required]
        public DateTime EndDate { get; set; }

    [Required]
    [ForeignKey("Project")]
    public int ProjectFK { get; set; }
  }
}
