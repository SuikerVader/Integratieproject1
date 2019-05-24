using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Integratieproject1.Domain.Projects;

namespace Integratieproject1.Domain.Surveys
{
    public class Survey
    {
      [Key]
        public int SurveyId { get; set; }
    [Required]
        public string Title { get; set; }
    public Phase Phase { get; set; }
    public ICollection<Question> Questions { get; set; }
  }
}
