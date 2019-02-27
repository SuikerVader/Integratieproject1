using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Integratieproject1.Models.Projects;

namespace Integratieproject1.Models.Surveys
{
    public class Survey
    {
      [Key]
        public int SurveyId { get; set; }
    [Required]
        public string Title { get; set; }
    [Required] public Phase Phase { get; set; }
    public IList<Question> Questions { get; set; }
    
    /*[ForeignKey("Fase")]
    public int FaseFK { get; set; }*/
  }
}
