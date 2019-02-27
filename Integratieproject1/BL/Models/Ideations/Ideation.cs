using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Integratieproject1.BL.Models.Projects;

namespace Integratieproject1.BL.Models.Ideations
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
