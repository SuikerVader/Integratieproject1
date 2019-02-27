using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Integratieproject1.BL.Models.IoT;

namespace Integratieproject1.BL.Models.Surveys
{
    public class Question
    { 
    [Key]
      public int QuestionId { get; set; }
        public int QuestionNr { get; set; }
        public string QuestionText { get; set; }
    [Required] public Survey Survey { get; set; }
    public IList<Answer> Answers { get; set; }
    public IList<IoTSetup> IoTSetups { get; set; }
    
    /*[ForeignKey("Survey")]
    public int SurveyFK { get; set; }*/
  }
}
