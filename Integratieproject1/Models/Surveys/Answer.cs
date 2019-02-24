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
        public String AnswerText { get; set; }
        public int TotalTimesChosen { get; set; }
    [Required]
    [ForeignKey("QuestionFK")]
    public int QuestionFK { get; set; }
  }
}
