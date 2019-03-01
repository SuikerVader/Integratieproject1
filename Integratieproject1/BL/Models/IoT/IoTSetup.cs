using System.ComponentModel.DataAnnotations;
using Integratieproject1.BL.Models.Datatypes;
using Integratieproject1.BL.Models.Ideations;
using Integratieproject1.BL.Models.Surveys;

namespace Integratieproject1.BL.Models.IoT
{
    public class IoTSetup
    { 
        [Key]
        public string Code { get; set; }
    [Required]
        public Position Position { get; set; }
        public Idea Idea { get; set; }
        public Question Question { get; set; }
    /*[ForeignKey("Idea")]
    public int IdeaFK { get; set; }
    [ForeignKey("Question")]
    public int QuestionFK { get; set; }*/
  }
}
