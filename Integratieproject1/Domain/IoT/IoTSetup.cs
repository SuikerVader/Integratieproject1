using System.ComponentModel.DataAnnotations;
using Integratieproject1.Domain.Datatypes;
using Integratieproject1.Domain.Ideations;
using Integratieproject1.Domain.Surveys;

namespace Integratieproject1.Domain.IoT
{
    public class IoTSetup
    { 
        [Key]
        public string Code { get; set; }
    [Required]
        public Position Position { get; set; }
        public Idea Idea { get; set; }
        public Question Question { get; set; }
   
  }
}
