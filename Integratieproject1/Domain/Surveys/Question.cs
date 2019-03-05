using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Integratieproject1.Domain.IoT;

namespace Integratieproject1.Domain.Surveys
{
    public class Question
    { 
    [Key]
      public int QuestionId { get; set; }
        public int QuestionNr { get; set; }
        public string QuestionText { get; set; }
    [Required] public Survey Survey { get; set; }
    public ICollection<Answer> Answers { get; set; }
    public ICollection<IoTSetup> IoTSetups { get; set; }
    
    /*[ForeignKey("Survey")]
    public int SurveyFK { get; set; }*/
  }
}
