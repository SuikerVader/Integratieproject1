using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Integratieproject1.Models.Surveys
{
    public class Question
    { 
    [Key]
      public int QuestionID { get; set; }
        public int QuestionNr { get; set; }
        public String QuestionText { get; set; }
    [Required]
    [ForeignKey("Survey")]
    public int SurveyFK { get; set; }
  }
}
