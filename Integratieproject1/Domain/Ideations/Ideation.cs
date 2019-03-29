using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Integratieproject1.Domain.Projects;

namespace Integratieproject1.Domain.Ideations
{
    public class Ideation
    {

    [Key]
    public int IdeationId { get; set; }
    [Required]
    public string CentralQuestion { get; set; }
    [Required]
    public bool InputIdeation { get; set; }
    //[Required] 
    public Phase Phase { get; set; }
    public ICollection<Reaction> Reactions { get; set; }
    public ICollection<Idea> Ideas { get; set; }
    
   
  }
}
