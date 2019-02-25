using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Integratieproject1.Models.Ideations
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
    [ForeignKey("Phase")]
    public int PhaseFk { get; set; }
  }
}
