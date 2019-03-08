using System.ComponentModel.DataAnnotations;

namespace Integratieproject1.Domain.Surveys
{
    public class Answer
    {
    [Key]
        public int AnswerId { get; set; }
        public string AnswerText { get; set; }
        [Required]
        public AnswerType AnswerType { get; set; }
        public int TotalTimesChosen { get; set; }
    [Required] public Question Question { get; set; }
    
    /*[ForeignKey("QuestionFK")]
    public int QuestionFK { get; set; }*/
  }
}
