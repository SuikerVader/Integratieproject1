using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Integratieproject1.Models.Surveys
{
    public class Answer
    {
    [Key]
        public int AnswerId { get; set; }
        public string AnswerText { get; set; }
        public int TotalTimesChosen { get; set; }
    [Required] public Question Question { get; set; }
    
    /*[ForeignKey("QuestionFK")]
    public int QuestionFK { get; set; }*/
  }
}
