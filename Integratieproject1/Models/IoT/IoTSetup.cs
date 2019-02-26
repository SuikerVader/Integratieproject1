using Integratieproject1.Models.Datatypes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Integratieproject1.Models.Ideations;
using Integratieproject1.Models.Surveys;

namespace Integratieproject1.Models.IoT
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
