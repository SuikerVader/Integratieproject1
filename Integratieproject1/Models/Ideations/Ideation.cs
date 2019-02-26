using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Integratieproject1.Models.Projects;

namespace Integratieproject1.Models.Ideations
{
    public class Ideation
    {

    [Key]
    public int IdeationId { get; set; }
    [Required]
    public string CentralQuestion { get; set; }
    [Required]
    public bool InputIdeation { get; set; }
    [Required] public Phase Phase { get; set; }
    public IList<Reaction> Reactions { get; set; }
    public IList<Idea> Ideas { get; set; }
    
    /*[ForeignKey("Phase")]
    public int PhaseFk { get; set; }*/
  }
}
