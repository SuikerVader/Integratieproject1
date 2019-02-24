using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Integratieproject1.Models
{
    public class Survey
    {
      [Key]
        public int SurveyId { get; set; }
    [Required]
        public String Title { get; set; }
    [Required]
    [ForeignKey("Fase")]
    public int FaseFK { get; set; }
  }
}
