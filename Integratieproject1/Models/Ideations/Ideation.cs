using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Integratieproject1.Models
{
    public class Ideation
    {

    [Key]
    public int IdeationId { get; set; }
    [Required]
    public String CentralQuestion { get; set; }
    [Required]
    public Boolean InputIdeation { get; set; }
    [Required]
    [ForeignKey("Fase")]
    public int FaseFK { get; set; }
  }
}
