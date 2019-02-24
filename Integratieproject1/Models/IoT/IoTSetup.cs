using Integratieproject1.Models.Datatypes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Integratieproject1.Models.IoT
{
    public class IoTSetup
    { 
        [Key]
        public String Code { get; set; }
    [Required]
        public Position Position { get; set; }

    [ForeignKey("Idea")]
    public int IdeaFK { get; set; }
    [ForeignKey("Question")]
    public int QuestionFK { get; set; }
  }
}
