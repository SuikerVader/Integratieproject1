using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Integratieproject1.Models.IoT;

namespace Integratieproject1.Models.Surveys
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
